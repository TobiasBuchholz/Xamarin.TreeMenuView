using Android.Support.V7.Widget;

namespace TreeMenuView.Android
{
    public sealed class IndexedItemHolderInfo : RecyclerView.ItemAnimator.ItemHolderInfo
    {
        public IndexedItemHolderInfo(RecyclerView.ItemAnimator.ItemHolderInfo info, int index)
        {
            Left = info.Left;
            Top = info.Top;
            Right = info.Right;
            Bottom = info.Bottom;
            Index = index;
        }

        public int Index { get; }
    }
}