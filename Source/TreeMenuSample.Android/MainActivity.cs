using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using TreeMenuSample.Shared;
using TreeMenuView.Android;
using TreeMenuView.Shared.Models;

namespace TreeMenuSample.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private FrameLayout _layout;
        private TextView _label;
        private TreeMenuView<Category, long> _treeMenu;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitLayout();
            InitTreeMenu();
            InitLabel();
            SetContentView(_layout);
        }

        private void InitLayout()
        {
            _layout = new FrameLayout(this);
            _layout.LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        }

        private void InitTreeMenu()
        {
            var cellHeight = Resources.GetDimensionPixelSize(Resource.Dimension.height_category_cell);
            _treeMenu = new TreeMenuView<Category, long>(this, (_, __) => new CategoryCell(this), cellHeight);
            _layout.AddView(_treeMenu.View, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

            _treeMenu.Items = Category.CreateSamples();
            _treeMenu.NodeSelected += HandleSelectedNode;
        }
        
        private void HandleSelectedNode(object sender, TreeNode<Category, long> node)
        {
            _label.Text = $"Selected category is {node.Data.Title}";
        }
        
        private void InitLabel()
        {
            _label = new TextView(this);
            _label.Gravity = GravityFlags.Bottom | GravityFlags.CenterHorizontal;
            _label.Text = $"Selected category is {_treeMenu.CurrentNode.Data.Title}";
            _layout.AddView(_label);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _treeMenu.NodeSelected -= HandleSelectedNode;
        }
    }
}

