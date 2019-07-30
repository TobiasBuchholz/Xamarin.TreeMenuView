using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public class TreeMenuView<TData, TKey> where TData : ITreeNodeData<TKey>
    {
        private readonly UICollectionView _collectionView;
        private readonly TreeMenuDataSource<TData, TKey> _dataSource;
        private readonly float _cellHeight;
        
        public TreeMenuView(string cellIdentifier, float cellHeight) 
        {
            var collectionViewDelegate = new TreeMenuCollectionViewDelegate();
            _cellHeight = cellHeight;
            _dataSource = new TreeMenuDataSource<TData, TKey>(cellHeight, cellIdentifier);
            _collectionView = new UICollectionView(CGRect.Empty, CreateCollectionViewLayout(cellHeight));
            _collectionView.BackgroundColor = UIColor.Clear;
            _collectionView.Bounces = false;
            _collectionView.DataSource = _dataSource;
            _collectionView.Delegate = collectionViewDelegate;
            collectionViewDelegate.OnItemSelected += (sender, args) => _dataSource.ItemSelected(_collectionView, args.IndexPath);
        }
        
        private TreeMenuCollectionViewFlowLayout CreateCollectionViewLayout(float itemHeight)
        {
            return new TreeMenuCollectionViewFlowLayout {
                MinimumLineSpacing = 0,
                ItemSize = CreateCollectionViewItemSize()
            };
        }

        private CGSize CreateCollectionViewItemSize()
        {
            return new CGSize(UIScreen.MainScreen.Bounds.Width, _cellHeight);
        }
        
        public void RegisterClassForCell(Type cellType, string reuseIdentifier)
        {
            _collectionView.RegisterClassForCell(cellType, reuseIdentifier);
        }
        
        public void RegisterNibForCell(UINib nib, string reuseIdentifier)
        {
            _collectionView.RegisterNibForCell(nib, reuseIdentifier);
        }

        public void RegisterNibForCell(UINib nib, NSString reuseIdentifier)
        {
            _collectionView.RegisterNibForCell(nib, reuseIdentifier);
        }

        public void ReloadData()
        {
            _collectionView.ReloadData();
        }

        public void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            var layout = (UICollectionViewFlowLayout) _collectionView.CollectionViewLayout;
            layout.ItemSize = CreateCollectionViewItemSize();
            _collectionView.CollectionViewLayout = layout;
        }

        public IEnumerable<TData> Items {
            set => _dataSource.CurrentNode = value.ToRootTreeNodes<TData, TKey>()[0];
        }

        public UIView View => _collectionView;

        public UIView BackgroundView {
            get => _collectionView.BackgroundView;
            set => _collectionView.BackgroundView = value;
        }
        
        public TreeNode<TData, TKey> CurrentNode {
            get => _dataSource.CurrentNode;
            set => _dataSource.CurrentNode = value;
        }
        
        public event EventHandler<TreeNode<TData, TKey>> NodeSelected {
            add => _dataSource.NodeSelected += value;
            remove => _dataSource.NodeSelected -= value;
        }

        public event EventHandler<nfloat> HeightWillChange {
            add => _dataSource.HeightWillChange += value;
            remove => _dataSource.HeightWillChange -= value;
        }
    }
}