using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public sealed class TreeMenuView<TData, TKey> : UICollectionView where TData : ITreeNodeData<TKey>
    {
        private TreeMenuDataSource<TData, TKey> _dataSource;
        
        public TreeMenuView(NSCoder coder) 
            : base(coder)
        {
        }

        public TreeMenuView(NSObjectFlag t) 
            : base(t)
        {
        }

        internal TreeMenuView(IntPtr handle) 
            : base(handle)
        {
        }

        public TreeMenuView(string cellIdentifier, float cellHeight) 
            : base(CGRect.Empty, CreateCollectionViewLayout(cellHeight))
        {
            Initialize(cellIdentifier);
        }
        
        private static TreeMenuCollectionViewFlowLayout CreateCollectionViewLayout(float itemHeight)
        {
            return new TreeMenuCollectionViewFlowLayout {
                MinimumLineSpacing = 0,
                ItemSize = new CGSize(UIScreen.MainScreen.Bounds.Width, itemHeight)
            };
        }

        private void Initialize(string cellIdentifier)
        {
            var collectionViewDelegate = new TreeMenuCollectionViewDelegate();
            _dataSource = new TreeMenuDataSource<TData, TKey>(((TreeMenuCollectionViewFlowLayout) CollectionViewLayout).ItemSize.Height, cellIdentifier);
            BackgroundColor = UIColor.Clear;
            Bounces = false;
            DataSource = _dataSource;
            Delegate = collectionViewDelegate;
            collectionViewDelegate.OnItemSelected += (sender, args) => _dataSource.ItemSelected(this, args.IndexPath);
        }

        public IEnumerable<TData> Items {
            set => _dataSource.CurrentNode = value.ToRootTreeNodes<TData, TKey>()[0];
        }
    }
}