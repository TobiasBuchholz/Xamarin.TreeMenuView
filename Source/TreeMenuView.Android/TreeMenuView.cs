using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;

namespace TreeMenuView.Android
{
    public sealed class TreeMenuView<TData, TKey> : RecyclerView where TData : ITreeNodeData<TKey>
    {
        private TreeMenuAdapter<TData, TKey> _adapter;
        
        public TreeMenuView(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public TreeMenuView(Context context) 
            : base(context)
        {
            Initialize(context);
        }

        public TreeMenuView(Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {
            Initialize(context);
        }

        public TreeMenuView(Context context, IAttributeSet attrs, int defStyle) 
            : base(context, attrs, defStyle)
        {
            Initialize(context);
        }

        private void Initialize(Context context)
        {
            _adapter = new TreeMenuAdapter<TData, TKey>(this);
			SetLayoutManager(new PredictiveLinearLayoutManager(context));
			SetAdapter(_adapter);
        }
        
        public IEnumerable<TData> Items {
            set => _adapter.CurrentNode = value.ToRootTreeNodes<TData, TKey>()[0];
        }
    }
}