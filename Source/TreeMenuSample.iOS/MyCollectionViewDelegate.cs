using System;
using Foundation;
using UIKit;

namespace TreeMenuSample.iOS
{
	public class MyCollectionViewDelegate : UICollectionViewDelegate
	{
		public event EventHandler<ItemSelectedEventArgs> OnItemSelected;

		public MyCollectionViewDelegate()
		{
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			OnItemSelected?.Invoke(collectionView, new ItemSelectedEventArgs { IndexPath = indexPath });
		}
	}
	
	public sealed class ItemSelectedEventArgs : System.EventArgs 
	{
		public NSIndexPath IndexPath { get; set; }
	}
}
