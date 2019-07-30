using System;
using CoreGraphics;
using Foundation;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public abstract class TreeMenuCell<TData> : UICollectionViewCell, ITreeMenuCell<TData>
    {
        [Export("initWithFrame:")]
        protected TreeMenuCell(CGRect frame) 
            : base (frame)
        {
        }
        
        public abstract TData Data { set; }
        public abstract ItemRelation Relation { set; }
    }
}