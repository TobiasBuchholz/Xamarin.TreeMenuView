using System;
using Foundation;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public sealed class TreeMenuDataSource<TData, TKey> : UICollectionViewDataSource where TData : ITreeNodeData<TKey>
    {
        private readonly TreeMenuDataSourceDelegate<TData, TKey> _delegate;
        
        public TreeMenuDataSource(nfloat cellHeight) 
        {
            _delegate = new TreeMenuDataSourceDelegate<TData, TKey>(cellHeight, GetCell, HandleItemStateChanged);
        }

        private static UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath, ItemRelation relation, TData data)
        {
            var cell = (TreeMenuCell) collectionView.DequeueReusableCell(TreeMenuCell.CellIdentifier, indexPath);
            cell.Title = data.Title;
            cell.Relation = relation;
            return cell;
        }
        
        private static void HandleItemStateChanged(UICollectionViewCell collectionViewCell, ItemRelation relation)
        {
            var cell = (TreeMenuCell) collectionViewCell;
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