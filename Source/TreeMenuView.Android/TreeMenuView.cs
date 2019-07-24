using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;

namespace TreeMenuView.Android
{
    public sealed class TreeMenuView<TData, TKey> : RecyclerView where TData : ITreeNodeData<TKey>
    {
        private readonly TreeMenuAdapter<TData, TKey> _adapter;
        
        public TreeMenuView(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public TreeMenuView(Context context, Func<ViewGroup, int, TreeMenuCell> cellSelector, int itemHeight) 
            : base(context)
        {
            _adapter = new TreeMenuAdapter<TData, TKey>(this, cellSelector, itemHeight);
            SetLayoutManager(new PredictiveLinearLayoutManager(context)); 
            SetAdapter(_adapter);
        }

        public IEnumerable<TData> Items {
            set => _adapter.CurrentNode = value.ToRootTreeNodes<TData, TKey>()[0];
        }
    }
}