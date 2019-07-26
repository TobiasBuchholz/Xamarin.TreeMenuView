using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Support.V7.Util;
using Android.Support.V7.Widget;
using Android.Views;
using TreeMenuView.Shared.Models;

namespace TreeMenuView.Android
{
    public sealed class TreeMenuAdapterDelegate<T, TKey> where T : ITreeNodeData<TKey>
    {
        private readonly int _itemHeight;
        private readonly RecyclerView _recyclerView;
        private readonly Func<ViewGroup, int, RecyclerView.ViewHolder> _viewHolderSelector;
        private readonly Action<RecyclerView.ViewHolder, ItemRelation, T> _viewHolderBound;
        private readonly Action<RecyclerView.ViewHolder, ItemRelation> _itemStateChanged;
        private readonly ItemAnimator _itemAnimator;
        private readonly TreeMenuItemCollection<T, TKey> _itemCollection;
        private TreeNode<T, TKey> _currentNode;

        public TreeMenuAdapterDelegate(
            int itemHeight,
            RecyclerView recyclerView,
            Func<ViewGroup, int, RecyclerView.ViewHolder> viewHolderSelector,
            Action<RecyclerView.ViewHolder, ItemRelation, T> viewHolderBound,
            Action<RecyclerView.ViewHolder, ItemRelation> itemStateChanged)
        {
            _itemHeight = itemHeight;
            _recyclerView = recyclerView;
            _viewHolderSelector = viewHolderSelector;
            _viewHolderBound = viewHolderBound;
            _itemStateChanged = itemStateChanged;
            _itemCollection = new TreeMenuItemCollection<T, TKey>();
            _itemAnimator = new ItemAnimator();
            _recyclerView.SetItemAnimator(_itemAnimator);
        }

        public void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var relation = _itemCollection.GetItemRelation(position);
            var data = _itemCollection.GetItemData(position);
            _viewHolderBound(holder, relation, data);
        }

        public RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return _viewHolderSelector(parent, viewType);
        }

        public void OnItemSelected(int index)
        {
            // attention: if SetCurrentNodeWithAnimationAsync(...) throws an exception it will be ignored because of this
            SetCurrentNodeWithAnimationAsync(_itemCollection[index]).Ignore();
        }

        private async Task SetCurrentNodeWithAnimationAsync(TreeNode<T, TKey> selectedNode)
        {
            var diffResult = TreeMenuDiff.Calculate(CurrentNode, selectedNode);
            diffResult
                .ChangedIndexes
                .Select(x => (cell: _recyclerView.FindViewHolderForLayoutPosition(x.Index), relation: x.Relation))
                .Where(x => x.cell != null)
                .ForEach((_,x) => _itemStateChanged(x.cell, x.relation));
            
            CurrentNode = selectedNode;
            _itemAnimator.DiffResult = diffResult;
            await AnimateDiffAsync(diffResult);
            NodeSelected?.Invoke(this, selectedNode);
        }
        
        private async Task AnimateDiffAsync(TreeMenuDiffResult treeMenuDiffResult)
        {
            await DeleteItemsIfNeededAsync(treeMenuDiffResult.RemovedIndexes.ToArray());
            await InsertItemsIfNeededAsync(treeMenuDiffResult.AddedIndexes.ToArray());
        }

        private Task DeleteItemsIfNeededAsync(IReadOnlyList<int> indexes)
        {
            return indexes.Any() ? DeleteItemsAsync(indexes) : Task.CompletedTask;
        }

        private Task DeleteItemsAsync(IEnumerable<int> indexes)
        {
            // unfortunately removing the indexes the same way like they are added in InsertItemsAsync()
            // results in a crash for some selected indexes, so DiffUtil comes to the rescue
            var oldIndexes = Enumerable.Range(0, ItemCount).ToArray();
            var newIndexes = oldIndexes.Except(indexes).ToArray();
            var callback = new IndexDiffUtilCallback(oldIndexes, newIndexes);
            var adapterDiff = DiffUtil.CalculateDiff(callback);

            ItemCount = newIndexes.Length;
            InvokeHeightWillChange();

            adapterDiff.DispatchUpdatesTo(_recyclerView.GetAdapter());
            return Task.Delay(TimeSpan.FromMilliseconds(300));
        }

        private void InvokeHeightWillChange()
        {
            HeightWillChange?.Invoke(this, ItemCount * _itemHeight);
        }

        private Task InsertItemsIfNeededAsync(IReadOnlyCollection<int> indexes)
        {
            return indexes.Any() ? InsertItemsAsync(indexes) : Task.CompletedTask;
        }

        private Task InsertItemsAsync(IReadOnlyCollection<int> indexes)
        {
            ItemCount = _itemCollection.Count;
            InvokeHeightWillChange();
            NotifyItemsInserted(indexes);
            return Task.Delay(TimeSpan.FromMilliseconds(300));
        }

        private void NotifyItemsInserted(IEnumerable<int> indexes)
        {
            var consecutiveGroups = indexes.GroupConsecutive();
            foreach(var group in consecutiveGroups) {
                var array = group.ToArray();
                _recyclerView.GetAdapter().NotifyItemRangeInserted(array.First(), array.Count());
            }
        }
        
        public int ItemCount { get; private set; }

        public TreeNode<T, TKey> CurrentNode {
            get => _currentNode;
            set {
                _currentNode = value;
                _itemCollection.CurrentNode = value;
                ItemCount = ItemCount == 0 ? _itemCollection.Count : ItemCount;
            }
        }

        public event EventHandler<TreeNode<T, TKey>> NodeSelected;
        public event EventHandler<int> HeightWillChange;
    }
}