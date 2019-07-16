using System;
using System.Collections.Generic;
using System.Linq;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;
using Xunit;

namespace TreeMenuView.UnitTests
{
    public class TreeNodesFixture
    {
        [Fact]
        public void converts_list_to_tree_node()
        {
            var items = CreateSimpleLongItems().ToList();
            var node0 = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            var node1 = items.ToRootTreeNodes<SimpleLongItem, long>()[1];
            
            Assert.Equal(0, node0.Id);
            Assert.Equal(8, node1.Id);
            Assert.Equal(new [] { items[1], items[2] }, node0.ChildNodes.Select(x => x.Data));
            Assert.Equal(new [] { items[3], items[4], items[5] }, node0.ChildNodes[0].ChildNodes.Select(x => x.Data));
            Assert.Equal(new [] { items[6], items[7] }, node0.ChildNodes[1].ChildNodes.Select(x => x.Data));
            Assert.Equal(new [] { items[9], items[10] }, node1.ChildNodes.Select(x => x.Data));
        }

        private static IEnumerable<SimpleLongItem> CreateSimpleLongItems()
        {
            yield return new SimpleLongItem(TreeNode.ParentIdNone, 0);
            yield return new SimpleLongItem(0, 1);
            yield return new SimpleLongItem(0, 2);
            yield return new SimpleLongItem(1, 3);
            yield return new SimpleLongItem(1, 4);
            yield return new SimpleLongItem(1, 5);
            yield return new SimpleLongItem(2, 6);
            yield return new SimpleLongItem(2, 7);
            yield return new SimpleLongItem(TreeNode.ParentIdNone, 8);
            yield return new SimpleLongItem(8, 9);
            yield return new SimpleLongItem(8, 10);
        }

        [Fact]
        public void finds_child_node_with_id_in_node()
        {
            var items = CreateSimpleLongItems().ToList();
            var node = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            
            Assert.Equal(1, node.FindChildNodeWithId(1).Id);
            Assert.Equal(2, node.FindChildNodeWithId(2).Id);
            Assert.Equal(3, node.FindChildNodeWithId(3).Id);
            Assert.Equal(4, node.FindChildNodeWithId(4).Id);
            Assert.Equal(5, node.FindChildNodeWithId(5).Id);
            Assert.Equal(6, node.FindChildNodeWithId(6).Id);
            Assert.Equal(7, node.FindChildNodeWithId(7).Id);
        }

        [Fact]
        public void throws_argument_exception_if_node_does_not_contain_child_node()
        {
            var items = CreateSimpleLongItems().ToList();
            var node = items.ToRootTreeNodes<SimpleLongItem, long>()[0];

            Assert.Throws<ArgumentException>(() => node.FindChildNodeWithId(1337));
        }

        [Fact]
        public void finds_node_with_id_in_node()
        {
            var items = CreateSimpleLongItems().ToList();
            var node = items.ToRootTreeNodes<SimpleLongItem, long>()[0].FindChildNodeWithId(7);
            
            Assert.Equal(0, node.FindNodeWithId(0).Id);
            Assert.Equal(1, node.FindNodeWithId(1).Id);
            Assert.Equal(2, node.FindNodeWithId(2).Id);
            Assert.Equal(3, node.FindNodeWithId(3).Id);
            Assert.Equal(4, node.FindNodeWithId(4).Id);
            Assert.Equal(5, node.FindNodeWithId(5).Id);
            Assert.Equal(6, node.FindNodeWithId(6).Id);
        }

        [Fact]
        public void can_be_checked_for_equality()
        {
            var items = new List<SimpleLongItem> {
                new SimpleLongItem(-1, 0),
                new SimpleLongItem(0, 1),
                new SimpleLongItem(0, 2),
                new SimpleLongItem(0, 3)
            };

            var node = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            var otherNode = items.ToRootTreeNodes<SimpleLongItem, long>()[0];

            Assert.True(node.IsDataCompletelyEqual(otherNode));
        }

        [Fact]
        public void is_not_equal_when_has_different_child_nodes_count()
        {
            var items = new List<SimpleLongItem> {
                new SimpleLongItem(-1, 0),
                new SimpleLongItem(0, 1),
                new SimpleLongItem(0, 2),
                new SimpleLongItem(0, 3)
            };

            var node = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            items.RemoveAt(3);
            var otherNode = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            
            Assert.False(node.IsDataCompletelyEqual(otherNode));
        }

        [Fact]
        public void is_not_equal_when_has_different_child_nodes()
        {
            var items = new List<SimpleLongItem> {
                new SimpleLongItem(-1, 0),
                new SimpleLongItem(0, 1),
                new SimpleLongItem(0, 2),
                new SimpleLongItem(0, 3)
            };

            var node = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            items[3] = new SimpleLongItem(0, 10);
            var otherNode = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            
            Assert.False(node.IsDataCompletelyEqual(otherNode));
        }

        [Fact]
        public void converts_list_to_tree_node_when_key_is_string()
        {
            var items = CreateSimpleStringItems().ToList();
            var node0 = items.ToRootTreeNodes<SimpleStringItem, string>()[0];
            var node1 = items.ToRootTreeNodes<SimpleStringItem, string>()[1];
            
            Assert.Equal("0", node0.Id);
            Assert.Equal("8", node1.Id);
            Assert.Equal(new [] { items[1], items[2] }, node0.ChildNodes.Select(x => x.Data));
            Assert.Equal(new [] { items[3], items[4], items[5] }, node0.ChildNodes[0].ChildNodes.Select(x => x.Data));
            Assert.Equal(new [] { items[6], items[7] }, node0.ChildNodes[1].ChildNodes.Select(x => x.Data));
            Assert.Equal(new [] { items[9], items[10] }, node1.ChildNodes.Select(x => x.Data));
        }
        
        private static IEnumerable<SimpleStringItem> CreateSimpleStringItems()
        {
            yield return new SimpleStringItem(TreeNode.ParentIdNone.ToString(), "0");
            yield return new SimpleStringItem("0", "1");
            yield return new SimpleStringItem("0", "2");
            yield return new SimpleStringItem("1", "3");
            yield return new SimpleStringItem("1", "4");
            yield return new SimpleStringItem("1", "5");
            yield return new SimpleStringItem("2", "6");
            yield return new SimpleStringItem("2", "7");
            yield return new SimpleStringItem(TreeNode.ParentIdNone.ToString(), "8");
            yield return new SimpleStringItem("8", "9");
            yield return new SimpleStringItem("8", "10");
        }
    }
}