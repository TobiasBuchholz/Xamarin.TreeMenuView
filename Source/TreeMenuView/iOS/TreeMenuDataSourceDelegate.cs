using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using TreeMenuView.iOS.Extensions;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public class TreeMenuDataSourceDelegate<T, TKey> where T : ITreeNodeData<TKey>
    {
        private readonly nfloat _cellHeight;
        private readonly Func<UICollectionView, NSIndexPath, ItemRelation, T, UICollectionViewCell> _cellSelector;
        private readonly Action<UICollectionViewCell, ItemRelation> _itemStateChanged;
        private readonly TreeMenuItemCollection<T, TKey> _itemCollection;
        private TreeNode<T, TKey> _currentNode;

        public TreeMenuDataSourceDelegate(
            nfloat cellHeight,
            Func<UICollectionView, NSIndexPath, ItemRelation, T, UICollectionViewCell> cellSelector,
            Action<UICollectionViewCell, ItemRelation> itemStateChanged)
        {
            _cellHeight = cellHeight;
            _cellSelector = cellSelector;
            _itemStateChanged = itemStateChanged;
            _itemCollection = new TreeMenuItemCollection<T, TKey>();
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var index = indexPath.Row;
            var relation = _itemCollection.GetItemRelation(index);
            var data = _itemCollection.GetItemData(index);
            return _cellSelector(collectionView, indexPath, relation, data);
        }
        
        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // attention: if SetCurrentNodeWithAnimationAsync(...) throws an exception it will be ignored because of this
            SetCurrentNodeWithAnimationAsync(collectionView, _itemCollection[indexPath.Row]).Ignore();
        }

        private async Task SetCurrentNodeWithAnimationAsync(UICollectionView collectionView, TreeNode<T, TKey> selectedNode)
        {
            var diffResult = TreeMenuDiff.Calculate(CurrentNode, selectedNode);
            diffResult
                .ChangedIndexes
                .Select(x => (cell: collectionView.CellForItem(NSIndexPath.FromRowSection(x.Index, 0)), relation: x.Relation))
                .Where(x => x.cell != null)
                .ForEach((_,x) => _itemStateChanged(x.cell, x.relation));
            
            CurrentNode = selectedNode;
            await AnimateDiffAsync(collectionView, diffResult);
            NodeSelected?.Invoke(this, selectedNode);
        }

        private async Task AnimateDiffAsync(UICollectionView collectionView, TreeMenuDiffResult treeMenuDiffResult)
        {
            await DeleteItemsAsync(collectionView, treeMenuDiffResult.RemovedIndexes.ToArray());
            await InsertItemsAsync(collectionView, treeMenuDiffResult.AddedIndexes.ToArray());
        }

        private async Task DeleteItemsAsync(UICollectionView collectionView, IReadOnlyCollection<int> indexes)
        {
            await collectionView.PerformBatchUpdatesAsync(() => {
                ItemsCount -= indexes.Count;
                InvokeHeightWillChange();
                collectionView.DeleteItems(indexes.ToNSIndexPaths().ToArray());
            });
        }

        private void InvokeHeightWillChange()
        {
            HeightWillChange?.Invoke(this, ItemsCount * _cellHeight);
        }

        private async Task InsertItemsAsync(UICollectionView collectionView, IReadOnlyCollection<int> indexes)
        {
            await collectionView.PerformBatchUpdatesAsync(() => {
                ItemsCount = _itemCollection.Count;
                InvokeHeightWillChange();
                collectionView.InsertItems(indexes.ToNSIndexPaths().ToArray());
            });
        }
        
        public int ItemsCount { get; private set; }
        public int NumberOfSections => 1;
        
        public TreeNode<T, TKey> CurrentNode {
            get => _currentNode;
            set {
                _currentNode = value;
                _itemCollection.CurrentNode = value;
                ItemsCount = ItemsCount == 0 ? _itemCollection.Count : ItemsCount;
            }
        }

        public event EventHandler<TreeNode<T, TKey>> NodeSelected;
        public event EventHandler<nfloat> HeightWillChange;
    }
}