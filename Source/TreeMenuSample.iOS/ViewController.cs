using System;
using CoreGraphics;
using TreeMenuSample.Shared;
using TreeMenuView.iOS;
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

            var treeMenuView = new TreeMenuView<Category, long>(CGRect.Empty, 56);
            treeMenuView.TranslatesAutoresizingMaskIntoConstraints = false;
            View.AddSubview(treeMenuView);

            treeMenuView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            treeMenuView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            treeMenuView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            treeMenuView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;

            treeMenuView.Items = Category.CreateSamples();
        }
    }
}