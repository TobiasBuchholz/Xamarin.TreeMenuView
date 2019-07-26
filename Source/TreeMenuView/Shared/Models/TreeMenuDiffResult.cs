using System.Collections.Generic;

namespace TreeMenuView.Shared.Models
{
    public sealed class TreeMenuDiffResult
    {
        public TreeMenuDiffResult(
            IEnumerable<int> addedIndexes, 
            IEnumerable<int> removedIndexes,
            IEnumerable<(int, ItemRelation)> changedIndexes)
        {
            AddedIndexes = addedIndexes;
            RemovedIndexes = removedIndexes;
            ChangedIndexes = changedIndexes;
        }

        public override string ToString()
        {
            return $"added: [{string.Join(",", AddedIndexes)}] | removed: [{string.Join(",", RemovedIndexes)}] | moved: [{string.Join(",", ChangedIndexes)}]";
        }

        public IEnumerable<int> AddedIndexes { get; }
        public IEnumerable<int> RemovedIndexes { get; }
        public IEnumerable<(int Index, ItemRelation Relation)> ChangedIndexes { get; }
    }
}