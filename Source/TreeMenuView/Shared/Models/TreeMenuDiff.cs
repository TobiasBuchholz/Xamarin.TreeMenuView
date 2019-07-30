using System.Collections.Generic;
using System.Linq;
using TreeMenuView.Extensions.System.Linq;

namespace TreeMenuView.Shared.Models
{
public static class TreeMenuDiff
    {
        public static TreeMenuDiffResult Calculate<T, TKey>(TreeNode<T, TKey> node, TreeNode<T, TKey> otherNode) where T : ITreeNodeData<TKey>
        {
            return node.IsRoot || otherNode.IsRoot
                ? CalculateForLevel0(node, otherNode)
                : CalculateForOtherLevels(node, otherNode);
        }

        private static TreeMenuDiffResult CalculateForLevel0<T, TKey>(TreeNode<T, TKey> node, TreeNode<T, TKey> otherNode) where T : ITreeNodeData<TKey>
        {
            var diffResult = CalculateForOtherLevels(node, otherNode);
            return node.IsRoot
                ? new TreeMenuDiffResult(
                    0.ToSingleArray().Concat(diffResult.AddedIndexes),
                    diffResult.RemovedIndexes.Decrement(),
                    diffResult.ChangedIndexes.Skip(1).Select((x, _) => (x.Index - 1, ItemRelation.Selected)))
                : new TreeMenuDiffResult(
                    diffResult.AddedIndexes.Decrement(),
                    0.ToSingleArray().Concat(diffResult.RemovedIndexes),
                    diffResult.ChangedIndexes.Skip(1));
        }

        private static TreeMenuDiffResult CalculateForOtherLevels<T, TKey>(TreeNode<T, TKey> node, TreeNode<T, TKey> otherNode) where T : ITreeNodeData<TKey>
        {
            var flattenedNodes = node.Flatten().ToList();
            var otherFlattenedNodes = otherNode.Flatten().ToList();

            var removedNodes = flattenedNodes.Except(otherFlattenedNodes).ToList();
            var addedNodes = otherFlattenedNodes.Except(flattenedNodes);
            var movingNodes = flattenedNodes.Except(removedNodes);

            var removedIndexes = removedNodes.Select(x => flattenedNodes.FindIndex(0, y => x.Id.Equals(y.Id)));
            var addedIndexes = addedNodes.Select(x => otherFlattenedNodes.FindIndex(0, y => x.Id.Equals(y.Id)));
            var movingIndexes = movingNodes.Select(x => flattenedNodes.FindIndex(0, y => x.Id.Equals(y.Id)));
            var changedIndexes = GetChangedIndexes(otherNode, flattenedNodes, movingIndexes);
            return new TreeMenuDiffResult(addedIndexes, removedIndexes, changedIndexes);
        }

        private static IEnumerable<(int, ItemRelation)> GetChangedIndexes<T, TKey>(TreeNode<T, TKey> otherNode, IList<TreeNode<T, TKey>> flattenedNodes, IEnumerable<int> movingIndexes) where T : ITreeNodeData<TKey>
        {
            var index = flattenedNodes.IndexOf(otherNode);
            return movingIndexes.Select(i => (i, i == index ? ItemRelation.Selected : GetRelationForPreviousNode(flattenedNodes[i], otherNode)));
        }

        private static ItemRelation GetRelationForPreviousNode<T, TKey>(TreeNode<T, TKey> changedNode, TreeNode<T, TKey> otherNode) where T : ITreeNodeData<TKey>
        {
            return changedNode.IsRoot 
                ? ItemRelation.Root 
                : otherNode.ChildNodes.Contains(changedNode) ? ItemRelation.Child : ItemRelation.Parent;
        }
    }
}