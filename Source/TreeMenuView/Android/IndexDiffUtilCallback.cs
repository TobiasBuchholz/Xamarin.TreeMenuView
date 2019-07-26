using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Util;

namespace TreeMenuView.Android
{
    public sealed class IndexDiffUtilCallback : DiffUtil.Callback
    {
        private readonly IList<int> _oldIndexes;
        private readonly IList<int> _newIndexes;

        public IndexDiffUtilCallback(IList<int> oldIndexes, IList<int> newIndexes)
        {
            _oldIndexes = oldIndexes;
            _newIndexes = newIndexes;
        }

        public override bool AreContentsTheSame(int oldItemPosition, int newItemPosition)
        {
            return _oldIndexes?[oldItemPosition] == _newIndexes?[newItemPosition];
        }

        public override bool AreItemsTheSame(int oldItemPosition, int newItemPosition)
        {
            return _oldIndexes?[oldItemPosition] == _newIndexes?[newItemPosition];
        }

        public override int OldListSize => _oldIndexes?.Count() ?? 0;
        public override int NewListSize => _newIndexes?.Count() ?? 0;
    }
}