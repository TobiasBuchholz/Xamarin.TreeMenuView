using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TreeMenuSample.Shared;
using TreeMenuView.Android;
using TreeMenuView.Shared.Extensions;

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
            var recyclerView = new RecyclerView(this);
			var adapter = new TreeMenuAdapter<Category, long>(recyclerView);
			recyclerView.SetLayoutManager(new PredictiveLinearLayoutManager(this));
			recyclerView.SetAdapter(adapter);
            layout.AddView(recyclerView, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            
            var categories = Category.CreateSamples();
            var rootNode = categories.ToRootTreeNodes<Category, long>()[0];
            adapter.CurrentNode = rootNode;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

