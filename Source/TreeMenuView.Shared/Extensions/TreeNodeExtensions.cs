using System;
using System.Collections.Generic;
using System.Linq;
using TreeMenuView.Shared.Models;

namespace TreeMenuView.Shared.Extensions
{
    public static class TreeNodeExtensions
    {
        public static IList<TreeNode<T, TKey>> ToRootTreeNodes<T, TKey>(this IEnumerable<T> @this) where T : ITreeNodeData<TKey>
        {
            var nodesDict = @this.ToDictionary(x => x.Id, x => new TreeNode<T, TKey>(x));
            var rootNodes = new List<TreeNode<T, TKey>>();

            foreach(var pair in nodesDict) {
                var node = pair.Value;
                if(IsParentIdNone(node.ParentId)) {
                    rootNodes.Add(node);
                } else {
                    if(nodesDict.ContainsKey(node.ParentId)) {
                        nodesDict[node.ParentId].AddChildNode(node);
                    }
                }
            }
            return rootNodes;
        }

        private static bool IsParentIdNone(object id)
        {
            switch(id) {
                case int x:
                    return x == TreeNode.ParentIdNone;
                case long x:
                    return x == TreeNode.ParentIdNone;
                case float x:
                    return x.NearlyEquals(TreeNode.ParentIdNone);
                case string x:
                    return x == TreeNode.ParentIdNone.ToString();
                default:
                    throw new ArgumentException($"Could not parse tree node because type of key {id} is unknown");
            }
        }
    }
}