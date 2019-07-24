using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;

namespace TreeMenuView.Android
{
    public sealed class TreeMenuView<TData, TKey> where TData : ITreeNodeData<TKey>
    {
        private readonly RecyclerView _recyclerView;
        private readonly TreeMenuAdapter<TData, TKey> _adapter;
        
        public TreeMenuView(Context context, Func<ViewGroup, int, TreeMenuCell> cellSelector, int itemHeight) 
        {
            _recyclerView = new RecyclerView(context);
            _adapter = new TreeMenuAdapter<TData, TKey>(_recyclerView, cellSelector, itemHeight);
            _recyclerView.SetLayoutManager(new PredictiveLinearLayoutManager(context)); 
            _recyclerView.SetAdapter(_adapter);
        }

        public IEnumerable<TData> Items {
            set => _adapter.CurrentNode = value.ToRootTreeNodes<TData, TKey>()[0];
        }

        public View View => _recyclerView;
    }
}