using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using TreeMenuSample.Shared;
using TreeMenuView.Android;

namespace TreeMenuSample.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var layout = FindViewById<FrameLayout>(Resource.Id.layout);
            var cellHeight = Resources.GetDimensionPixelSize(Resource.Dimension.height_category_cell);
            var treeMenuView = new TreeMenuView<Category, long>(this, (_,__) => new CategoryCell(this), cellHeight);
            layout.AddView(treeMenuView, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            
            treeMenuView.Items = Category.CreateSamples();
        }
    }
}

