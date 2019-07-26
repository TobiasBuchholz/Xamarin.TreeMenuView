using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using TreeMenuView.Shared.Models;

namespace TreeMenuView.Android
{
    public class TreeMenuAdapter<TData, TKey> : RecyclerView.Adapter 
        where TData : ITreeNodeData<TKey>
    {
        private readonly TreeMenuAdapterDelegate<TData, TKey> _delegate;
        private readonly Func<ViewGroup, int, TreeMenuCell> _cellSelector;
        
        public TreeMenuAdapter(RecyclerView recyclerView, Func<ViewGroup, int, TreeMenuCell> cellSelector, int itemHeight)
        {
            _cellSelector = cellSelector;
            _delegate = new TreeMenuAdapterDelegate<TData, TKey>(
                itemHeight,
                recyclerView,
                SelectViewHolder,
                HandleViewHolderBound,
                HandleItemStateChanged);
        }

        private RecyclerView.ViewHolder SelectViewHolder(ViewGroup parent, int viewType)
        {
            return new TreeMenuAdapterViewHolder(_cellSelector(parent, viewType), OnItemSelected);
        }
        
        private void OnItemSelected(int index)
        {
            ItemSelected?.Invoke(this, index);
            _delegate.OnItemSelected(index);    
        }

        private static void HandleViewHolderBound(RecyclerView.ViewHolder holder, ItemRelation relation, TData data)
        {
            var viewHolder = (TreeMenuAdapterViewHolder) holder;
            viewHolder.Cell.Title = data.Title;
            viewHolder.Cell.Relation = relation;
        }

        private static void HandleItemStateChanged(RecyclerView.ViewHolder holder, ItemRelation relation)
        {
            var viewHolder = (TreeMenuAdapterViewHolder) holder;
            viewHolder.Cell.Relation = relation;
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _delegate.OnBindViewHolder(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return _delegate.OnCreateViewHolder(parent, viewType);
        }

        public event EventHandler<int> ItemSelected;
        public override int ItemCount => _delegate.ItemCount;
        
        public TreeNode<TData, TKey> CurrentNode {
            get => _delegate.CurrentNode;
            set => _delegate.CurrentNode = value;
        }
        
        public event EventHandler<int> HeightWillChange {
            add => _delegate.HeightWillChange += value;
            remove => _delegate.HeightWillChange -= value;
        }
        
        public event EventHandler<TreeNode<TData, TKey>> NodeSelected {
            add => _delegate.NodeSelected += value;
            remove => _delegate.NodeSelected -= value;
        }
    }
    
    internal class TreeMenuAdapterViewHolder : RecyclerView.ViewHolder
    {
        private readonly Action<int> _itemSelectedAction;
        
        internal TreeMenuAdapterViewHolder(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public TreeMenuAdapterViewHolder(View itemView, Action<int> itemSelectedAction = null) 
            : base(itemView)
        {
            _itemSelectedAction = itemSelectedAction;
            ItemView.Click += HandleItemClicked;
        }

        private void HandleItemClicked(object sender, EventArgs e)
        {
            if(AdapterPosition > -1) {
                _itemSelectedAction?.Invoke(AdapterPosition);   
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ItemView.Click -= HandleItemClicked;
        }

        public TreeMenuCell Cell => (TreeMenuCell) ItemView;
    }
}