using System;
using TreeMenuSample.Shared;
using TreeMenuView.iOS;
using TreeMenuView.Shared.Models;
using UIKit;

namespace TreeMenuSample.iOS
{
    public partial class ViewController : UIViewController
    {
        private UILabel _label;
        private TreeMenuView<Category, long> _treeMenu;
        
        public ViewController(IntPtr handle) 
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitTreeMenu();
            InitLabel();
        }

        private void InitTreeMenu()
        {
            _treeMenu = new TreeMenuView<Category, long>(CategoryCell.CellIdentifier, CategoryCell.Height);
            _treeMenu.RegisterClassForCell(typeof(CategoryCell), CategoryCell.CellIdentifier);
            _treeMenu.View.TranslatesAutoresizingMaskIntoConstraints = false;
            View.AddSubview(_treeMenu.View);

            _treeMenu.View.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            _treeMenu.View.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
            _treeMenu.View.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
            _treeMenu.View.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;

            _treeMenu.Items = Category.CreateSamples();
            _treeMenu.NodeSelected += HandleSelectedNode;
        }

        private void HandleSelectedNode(object sender, TreeNode<Category, long> node)
        {
            _label.Text = $"Selected category is {node.Data.Title}";
        }

        private void InitLabel()
        {
            _label = new UILabel();
            _label.TranslatesAutoresizingMaskIntoConstraints = false;
            _label.Text = $"Selected category is {_treeMenu.CurrentNode.Data.Title}";
            View.AddSubview(_label);

            _label.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            _label.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            _treeMenu.NodeSelected -= HandleSelectedNode;
        }
    }
}