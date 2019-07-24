using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public sealed class TreeMenuView<TData, TKey> where TData : ITreeNodeData<TKey>
    {
        private readonly UICollectionView _collectionView;
        private readonly TreeMenuDataSource<TData, TKey> _dataSource;
        
        public TreeMenuView(string cellIdentifier, float cellHeight) 
        {
            var collectionViewDelegate = new TreeMenuCollectionViewDelegate();
            _dataSource = new TreeMenuDataSource<TData, TKey>(cellHeight, cellIdentifier);
            _collectionView = new UICollectionView(CGRect.Empty, CreateCollectionViewLayout(cellHeight));
            _collectionView.BackgroundColor = UIColor.Clear;
            _collectionView.Bounces = false;
            _collectionView.DataSource = _dataSource;
            _collectionView.Delegate = collectionViewDelegate;
            collectionViewDelegate.OnItemSelected += (sender, args) => _dataSource.ItemSelected(_collectionView, args.IndexPath);
        }
        
        private static TreeMenuCollectionViewFlowLayout CreateCollectionViewLayout(float itemHeight)
        {
            return new TreeMenuCollectionViewFlowLayout {
                MinimumLineSpacing = 0,
                ItemSize = new CGSize(UIScreen.MainScreen.Bounds.Width, itemHeight)
            };
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

        public IEnumerable<TData> Items {
            set => _dataSource.CurrentNode = value.ToRootTreeNodes<TData, TKey>()[0];
        }

        public UIView View => _collectionView;
        
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