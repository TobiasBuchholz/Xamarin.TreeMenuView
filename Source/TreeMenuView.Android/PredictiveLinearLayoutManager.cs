using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;

namespace TreeMenuView.Android
{
    public sealed class PredictiveLinearLayoutManager : LinearLayoutManager
    {
        public PredictiveLinearLayoutManager(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public PredictiveLinearLayoutManager(Context context) 
            : base(context)
        {
        }

        public PredictiveLinearLayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) 
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public PredictiveLinearLayoutManager(Context context, int orientation, bool reverseLayout) 
            : base(context, orientation, reverseLayout)
        {
        }

        public override bool SupportsPredictiveItemAnimations()
        {
            return true;
        }
    }
}