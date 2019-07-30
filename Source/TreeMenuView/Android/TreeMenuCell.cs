using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using TreeMenuView.Shared.Models;

namespace TreeMenuView.Android
{
    public abstract class TreeMenuCell<TData> : FrameLayout, ITreeMenuCell<TData>
    {
        protected TreeMenuCell(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        protected TreeMenuCell(Context context) 
            : base(context)
        {
        }

        protected TreeMenuCell(Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {
        }

        protected TreeMenuCell(Context context, IAttributeSet attrs, int defStyleAttr) 
            : base(context, attrs, defStyleAttr)
        {
        }

        protected TreeMenuCell(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) 
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public abstract TData Data { set; }
        public abstract ItemRelation Relation { set; }
    }
}