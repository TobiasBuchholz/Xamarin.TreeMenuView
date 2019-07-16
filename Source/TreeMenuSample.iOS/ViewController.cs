using System;
using CoreGraphics;
using TreeMenuSample.Shared;
using TreeMenuView.iOS;
using TreeMenuView.Shared.Extensions;
using UIKit;

namespace TreeMenuSample.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var layout = CreateCollectionViewFlowLayout();
            var collectionView = new UICollectionView(CGRect.Empty, layout);
            var collectionViewDelegate = new MyCollectionViewDelegate();
            var dataSource = new TreeMenuDataSource<Category, long>(layout.ItemSize.Height);
            collectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            collectionView.RegisterClassForCell(typeof(TreeMenuCell), TreeMenuCell.CellIdentifier);
            collectionView.BackgroundColor = UIColor.Clear;
            collectionView.Bounces = false;
            collectionView.DataSource = dataSource;
            collectionView.Delegate = collectionViewDelegate;
            View.AddSubview(collectionView);

            collectionView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            collectionView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            collectionView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            collectionView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;

            var categories = Category.CreateSamples();
            var rootNode = categories.ToRootTreeNodes<Category, long>()[0];
            dataSource.CurrentNode = rootNode;
            collectionViewDelegate.OnItemSelected += (sender, args) => dataSource.ItemSelected(collectionView, args.IndexPath);
        }
        
        private static TreeMenuCollectionViewFlowLayout CreateCollectionViewFlowLayout()
        {
            return new TreeMenuCollectionViewFlowLayout {
                MinimumLineSpacing = 0,
                ItemSize = CreateCollectionViewItemSize()
            };
        }

        private static CGSize CreateCollectionViewItemSize()
        {
            return new CGSize(UIScreen.MainScreen.Bounds.Width, 56);
        }
    }
}