using System;
using TreeMenuSample.Shared;
using TreeMenuView.iOS;
using UIKit;

namespace TreeMenuSample.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) 
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var treeMenu = new TreeMenuView<Category, long>(CategoryCell.CellIdentifier, CategoryCell.Height);
            treeMenu.RegisterClassForCell(typeof(CategoryCell), CategoryCell.CellIdentifier);
            treeMenu.View.TranslatesAutoresizingMaskIntoConstraints = false;
            View.AddSubview(treeMenu.View);

            treeMenu.View.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            treeMenu.View.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            treeMenu.View.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            treeMenu.View.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;

            treeMenu.Items = Category.CreateSamples();
        }
    }
}