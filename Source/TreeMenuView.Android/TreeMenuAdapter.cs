using System;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TreeMenuView.Shared.Models;

namespace TreeMenuView.Android
{
    public sealed class TreeMenuAdapter<TData, TKey> : RecyclerView.Adapter 
        where TData : ITreeNodeData<TKey>
    {
        private readonly TreeMenuAdapterDelegate<TData, TKey> _delegate;
        
        public TreeMenuAdapter(RecyclerView recyclerView)
        {
            _delegate = new TreeMenuAdapterDelegate<TData, TKey>(
                recyclerView.Context.Resources.GetDimensionPixelSize(Resource.Dimension.height_tree_menu_item),
                recyclerView,
                SelectViewHolder,
                HandleViewHolderBound,
                HandleItemStateChanged);
        }

        private RecyclerView.ViewHolder SelectViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.tree_menu_item, parent, false);
            return new TreeMenuAdapterViewHolder(itemView, OnItemSelected);
        }
        
        private void OnItemSelected(int index)
        {
            ItemSelected?.Invoke(this, index);
            _delegate.OnItemSelected(index);    
        }

        private static void HandleViewHolderBound(RecyclerView.ViewHolder holder, ItemRelation relation, TData data)
        {
            var viewHolder = (TreeMenuAdapterViewHolder) holder;
            viewHolder.TitleLabel.Text = data.Title;
            viewHolder.Relation = relation;
        }

        private static void HandleItemStateChanged(RecyclerView.ViewHolder holder, ItemRelation relation)
        {
            var viewHolder = (TreeMenuAdapterViewHolder) holder;
            viewHolder.Relation = relation;
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
    
    public sealed class TreeMenuAdapterViewHolder : RecyclerView.ViewHolder
    {
        private readonly Action<int> _itemSelectedAction;
        
        public TreeMenuAdapterViewHolder(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public TreeMenuAdapterViewHolder(View itemView, Action<int> itemSelectedAction = null) 
            : base(itemView)
        {
            _itemSelectedAction = itemSelectedAction;
            TitleLabel = itemView.FindViewById<TextView>(Resource.Id.tree_menu_item_title_label);
            ItemView.Click += HandleItemClicked;
        }

        private void HandleItemClicked(object sender, EventArgs e)
        {
            if(AdapterPosition > -1) {
                _itemSelectedAction?.Invoke(AdapterPosition);   
            }
        }

        public TextView TitleLabel { get; }

        public ItemRelation Relation {
            set {
                switch(value) {
                    case ItemRelation.Root:
                        ApplyRootLayout();
                        break;
                    case ItemRelation.Parent:
                        ApplyParentLayout();
                        break;
                    case ItemRelation.Selected:
                        ApplySelectedLayout();
                        break;
                    case ItemRelation.Child:
                        ApplyChildLayout();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void ApplyRootLayout()
        {
            TitleLabel.SetTextColor(ContextCompat.GetColorStateList(TitleLabel.Context, Resource.Color.tree_menu_parent_item_text_selector));
            ItemView.SetBackgroundResource(Resource.Drawable.tree_menu_parent_item_background_selector);
        }

        private void ApplyParentLayout()
        {
            TitleLabel.SetTextColor(ContextCompat.GetColorStateList(TitleLabel.Context, Resource.Color.tree_menu_parent_item_text_selector));
            ItemView.SetBackgroundResource(Resource.Drawable.tree_menu_parent_item_background_selector);
        }

        private void ApplySelectedLayout()
        {
            TitleLabel.SetTextColor(ContextCompat.GetColorStateList(TitleLabel.Context, Resource.Color.tree_menu_selected_item_text_selector));
            ItemView.SetBackgroundResource(Resource.Drawable.tree_menu_selected_item_background_selector);
        }

        private void ApplyChildLayout()
        {
            TitleLabel.SetTextColor(ContextCompat.GetColorStateList(TitleLabel.Context, Resource.Color.tree_menu_child_item_text_selector));
            ItemView.SetBackgroundResource(Resource.Drawable.tree_menu_child_item_background_selector);
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ItemView.Click -= HandleItemClicked;
        }
    }
}