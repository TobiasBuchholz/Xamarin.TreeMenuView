using System.Collections.Generic;
using System.Linq;

namespace TreeMenuView.Shared.Models
{
    public sealed class TreeMenuItemCollection<T, TKey> where T : ITreeNodeData<TKey>
    {
        private IList<TreeNode<T, TKey>> _items;
        private IReadOnlyList<TreeNode<T, TKey>> _currentParentNodes;
        
        public TreeMenuItemCollection()
        {
            _items = new List<TreeNode<T, TKey>>();
        }

        public TreeNode<T, TKey> CurrentNode {
            set => Initialize(value);
        }

        private void Initialize(TreeNode<T, TKey> node)
        {
            _currentParentNodes = node.ParentNodes;
            _items = node.IsRoot ? node.ChildNodes.ToList() : node.ParentNodes.Append(node).Concat(node.ChildNodes).ToList();
        }

        public T GetItemData(int index)
        {
            return _items[index].Data;
        }

        public ItemRelation GetItemRelation(int index)
        {
            var node = _items[index];
            
            if(_items.First().ParentNodes.Any()) {
                return ItemRelation.Child;
            } else if(node.IsRoot) {
                return ItemRelation.Root;
            } else if(index < _currentParentNodes.Count) {
                return ItemRelation.Parent;
            } else if (index == _currentParentNodes.Count) {
                return ItemRelation.Selected;
            } else {
                return ItemRelation.Child;
            }
        }

        public TreeNode<T, TKey> this[int index] => _items[index];
        public int Count => _items.Count;
    }
}