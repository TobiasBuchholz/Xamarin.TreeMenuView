using System;
using Foundation;
using UIKit;

namespace TreeMenuView.iOS
{
	public class TreeMenuCollectionViewDelegate : UICollectionViewDelegate
	{
		public event EventHandler<ItemSelectedEventArgs> OnItemSelected;

		public TreeMenuCollectionViewDelegate()
		{
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			OnItemSelected?.Invoke(collectionView, new ItemSelectedEventArgs { IndexPath = indexPath });
		}
		
        public sealed class ItemSelectedEventArgs : System.EventArgs 
        {
            public NSIndexPath IndexPath { get; set; }
        }
	}
}
