using System;
using Foundation;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public sealed class TreeMenuDataSource<TData, TKey> : UICollectionViewDataSource where TData : ITreeNodeData<TKey>
    {
        private readonly TreeMenuDataSourceDelegate<TData, TKey> _delegate;
        private readonly string _cellIdentifier;
        
        public TreeMenuDataSource(nfloat cellHeight, string cellIdentifier) 
        {
            _delegate = new TreeMenuDataSourceDelegate<TData, TKey>(cellHeight, GetCell, HandleItemStateChanged);
            _cellIdentifier = cellIdentifier;
        }

        private UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath, ItemRelation relation, TData data)
        {
            var cell = (TreeMenuCell<TData>) collectionView.DequeueReusableCell(_cellIdentifier, indexPath);
            cell.Data = data;
            cell.Relation = relation;
            return cell;
        }
        
        private static void HandleItemStateChanged(UICollectionViewCell collectionViewCell, ItemRelation relation)
        {
            var cell = (TreeMenuCell<TData>) collectionViewCell;
            cell.Relation = relation;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return _delegate.GetCell(collectionView, indexPath);
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _delegate.ItemsCount;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return _delegate.NumberOfSections;
        }

        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            _delegate.ItemSelected(collectionView, indexPath);
        }

        public TreeNode<TData, TKey> CurrentNode {
            get => _delegate.CurrentNode;
            set => _delegate.CurrentNode = value;
        }
        
        public event EventHandler<TreeNode<TData, TKey>> NodeSelected {
            add => _delegate.NodeSelected += value;
            remove => _delegate.NodeSelected -= value;
        }
        
        public event EventHandler<nfloat> HeightWillChange {
            add => _delegate.HeightWillChange += value;
            remove => _delegate.HeightWillChange -= value;
        }
    }
}