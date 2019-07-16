using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TreeMenuView.Android
{
    public sealed class TreeMenuCell : FrameLayout
    {
        private TextView _titleLabel;
        private View _bottomDivider;
        
        public TreeMenuCell(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public TreeMenuCell(Context context) 
            : base(context)
        {
        }

        public TreeMenuCell(Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {
        }

        public TreeMenuCell(Context context, IAttributeSet attrs, int defStyleAttr) 
            : base(context, attrs, defStyleAttr)
        {
        }

        public TreeMenuCell(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) 
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        private void Initialize(Context context)
        {
            InitTitleLabel(context);
        }

        private void InitTitleLabel(Context context)
        {
            _titleLabel = new TextView(context);
            _titleLabel.Gravity = GravityFlags.Center;
        }
    }
}