using System;
using CoreGraphics;
using Foundation;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuView.iOS
{
    public abstract class TreeMenuCell : UICollectionViewCell, ITreeMenuCell
    {
        [Export("initWithFrame:")]
        protected TreeMenuCell(CGRect frame) 
            : base (frame)
        {
        }
        
        public abstract string Title { set; }
        public abstract ItemRelation Relation { set; }
    }
}