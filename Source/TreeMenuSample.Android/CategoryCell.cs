using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TreeMenuView.Android;
using TreeMenuView.Shared.Models;

namespace TreeMenuSample.Android
{
    public sealed class CategoryCell : TreeMenuCell
    {
        private TextView _titleLabel;
        
        public CategoryCell(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public CategoryCell(Context context) 
            : base(context)
        {
            InitHeight(context);
            InitTitleLabel(context);
            InitBottomDivider(context);
        }

        private void InitHeight(Context context)
        {
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, context.Resources.GetDimensionPixelSize(Resource.Dimension.height_category_cell));
        }

        private void InitTitleLabel(Context context)
        {
            _titleLabel = new TextView(context);
            _titleLabel.Gravity = GravityFlags.Center;
            _titleLabel.TextSize = 16;
            _titleLabel.SetSingleLine(true);
            AddView(_titleLabel);
        }
        
        private void InitBottomDivider(Context context)
        {
            var layoutParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, (int) (1 * context.Resources.DisplayMetrics.Density)) { Gravity = GravityFlags.Bottom };
            var divider = new View(context);
            divider.LayoutParameters = layoutParams;
            divider.SetBackgroundColor(Color.White);
            AddView(divider);
        }

        public override string Title {
            set => _titleLabel.Text = value;
        }

        public override ItemRelation Relation {
            set {
                switch(value) {
                    case ItemRelation.Root:
                        ApplyRootLayout();
                        break;
                    case ItemRelation.Parent:
                        ApplyParentLayout();
                        break;
                    case ItemRelation.Selected:
                        ApplySelectedLayout();
                        break;
                    case ItemRelation.Child:
                        ApplyChildLayout();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void ApplyRootLayout()
        {
            _titleLabel.SetTextColor(Color.White);
            SetBackgroundColor(Color.DarkGray);
        }

        private void ApplyParentLayout()
        {
            _titleLabel.SetTextColor(Color.White);
            SetBackgroundColor(Color.DarkGray);
        }

        private void ApplySelectedLayout()
        {
            _titleLabel.SetTextColor(Color.Black);
            SetBackgroundColor(Color.Green);
        }

        private void ApplyChildLayout()
        {
            _titleLabel.SetTextColor(Color.White);
            SetBackgroundColor(Color.LightGray);
        }
    }
}