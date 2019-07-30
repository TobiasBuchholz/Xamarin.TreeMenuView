using System;
using System.Collections.Generic;
using System.Linq;
using TreeMenuView.Extensions.System.Linq;

namespace TreeMenuView.Shared.Models
{
    public sealed class TreeNode<T, TKey> : TreeNode where T : ITreeNodeData<TKey>
    {
        private readonly List<TreeNode<T, TKey>> _parentNodes;
        private readonly List<TreeNode<T, TKey>> _childNodes;

        public TreeNode(T data)
        {
            Data = data;
            _parentNodes = new List<TreeNode<T, TKey>>();
            _childNodes = new List<TreeNode<T, TKey>>();
        }

        public TreeNode(T data, IEnumerable<TreeNode<T, TKey>> parentNodes, IEnumerable<TreeNode<T, TKey>> childNodes)
        {
            Data = data;
            _parentNodes = parentNodes.ToList();
            _childNodes = childNodes.ToList();
        }
        
        public override bool Equals(object obj)
        {
            if(obj is TreeNode<T, TKey> other) {
                return Equals(other);
            }
            return false;
        }

        private bool Equals(TreeNode<T, TKey> other)
        {
            return Data.Equals(other.Data);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

        public void AddChildNode(TreeNode<T, TKey> node)
        {
            node._parentNodes.AddRange(_parentNodes);
            node._parentNodes.Add(this);
            _childNodes.Add(node);
        }

        public IEnumerable<TreeNode<T, TKey>> Flatten()
        {
            return _parentNodes
                .Concat(new[] { this })
                .Concat(ChildNodes);
        }

        public TreeNode<T, TKey> FindChildNodeWithId(TKey id)
        {
            if(_childNodes.Any()) {
                foreach(var node in _childNodes) {
                    if(node.Id.Equals(id)) {
                        return node;
                    } else {
                        try {
                            return node.FindChildNodeWithId(id);
                        } catch(ArgumentException) {}
                    }
                }
            }
            throw new ArgumentException($"This node does not contains a child node with the id {id}");
        }

        public TreeNode<T, TKey> FindNodeWithId(TKey id)
        {
            var rootNode = GetRootNode();
            return rootNode.Id.Equals(id) ? rootNode : rootNode.FindChildNodeWithId(id);
        }

        private TreeNode<T, TKey> GetRootNode()
        {
            if(IsRoot) {
                return this;
            } else {
                foreach(var node in _parentNodes) {
                    return node.IsRoot ? node : node.GetRootNode();
                }
            }
            throw new Exception("This node has no root node which doesn't make sense :)");
        }

        public override string ToString()
        {
            return $"[TreeNode: Id={Id} | ParentId={ParentId} | Data={Data}]";
        }

        public bool IsDataCompletelyEqual(TreeNode<T, TKey> other)
        {
            return FullFlattenedData.SequenceEqualSafe(other.FullFlattenedData);
        }

        private IEnumerable<T> FullFlattenedData => 
            Data
                .ToSingleArray()
                .Concat(FullFlattenedParentData)
                .Concat(FullFlattenedChildrenData);

        private IEnumerable<T> FullFlattenedParentData =>
             _parentNodes
                .Select(x => x.Data)
                .Concat(_parentNodes.SelectMany(x => x._parentNodes.SelectMany(y => y.FullFlattenedParentData)));
        
        private IEnumerable<T> FullFlattenedChildrenData =>
            _childNodes
                .Select(x => x.Data)
                .Concat(_childNodes.SelectMany(x => x._childNodes.SelectMany(y => y.FullFlattenedChildrenData)));

        public T Data { get; }
        public TKey Id => Data.Id;
        public TKey ParentId => Data.ParentId;
        public bool IsRoot => !_parentNodes.Any();
        public IReadOnlyList<TreeNode<T, TKey>> ChildNodes => _childNodes.AsReadOnly();
        public IReadOnlyList<TreeNode<T, TKey>> ParentNodes => _parentNodes.AsReadOnly();
    }

    public abstract class TreeNode
    {
        public const long ParentIdNone = -1;
    }
}