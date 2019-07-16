using System.Collections.Generic;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;
using Xunit;

namespace TreeMenuView.UnitTests
{
    public class TreeMenuDiffFixture
    {
        [Fact]
        public void calculates_diff_from_level_0_to_level_1()
        {
            var node = CreateNodeOfLevel0();
            var otherNode = CreateNodeOfLevel1();
            var diffResult = TreeMenuDiff.Calculate(node, otherNode);
            Assert.Equal(new[] { 0, 2, 3, 4 }, diffResult.AddedIndexes);
            Assert.Equal(new[] { 1 }, diffResult.RemovedIndexes);
            Assert.Equal(new[] { (0, ItemRelation.Selected) }, diffResult.ChangedIndexes);
        }
        
        private static TreeNode<SimpleLongItem, long> CreateNodeOfLevel0()
        {
            return CreateSimpleLongItems().ToRootTreeNodes<SimpleLongItem, long>()[0];
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
            yield return new SimpleLongItem(4, 8);
        }
        
        private static TreeNode<SimpleLongItem, long> CreateNodeOfLevel1()
        {
            return CreateNodeOfLevel0().FindChildNodeWithId(1);
        }
        
        [Fact]
        public void calculates_diff_from_level_1_to_level_2()
        {
            var node = CreateNodeOfLevel1();
            var otherNode = CreateNodeOfLevel2();
            var diffResult = TreeMenuDiff.Calculate(node, otherNode);
            Assert.Equal(new[] { 3 }, diffResult.AddedIndexes);
            Assert.Equal(new[] { 2, 4 }, diffResult.RemovedIndexes);
            Assert.Equal(new[] { (0, ItemRelation.Root), (1, ItemRelation.Parent), (3, ItemRelation.Selected) }, diffResult.ChangedIndexes);
        }
        
        private static TreeNode<SimpleLongItem, long> CreateNodeOfLevel2()
        {
            return CreateNodeOfLevel0().FindChildNodeWithId(4);
        }
        
        [Fact]
        public void calculates_diff_from_level_2_to_level_3()
        {
            var node = CreateNodeOfLevel2();
            var otherNode = CreateNodeOfLevel3();
            var diffResult = TreeMenuDiff.Calculate(node, otherNode);
            Assert.Equal(new int[0], diffResult.AddedIndexes);
            Assert.Equal(new int[0], diffResult.RemovedIndexes);
            Assert.Equal(new[] { (0, ItemRelation.Root), (1, ItemRelation.Parent), (2, ItemRelation.Parent), (3, ItemRelation.Selected) }, diffResult.ChangedIndexes);
        }
        
        private static TreeNode<SimpleLongItem, long> CreateNodeOfLevel3()
        {
            return CreateNodeOfLevel0().FindChildNodeWithId(8);
        }
        
        [Fact]
        public void calculates_diff_from_level_3_to_level_2()
        {
            var node = CreateNodeOfLevel3();
            var otherNode = CreateNodeOfLevel2();
            var diffResult = TreeMenuDiff.Calculate(node, otherNode);
            Assert.Equal(new int[0], diffResult.AddedIndexes);
            Assert.Equal(new int[0], diffResult.RemovedIndexes);
            Assert.Equal(new[] { (0, ItemRelation.Root), (1, ItemRelation.Parent), (2, ItemRelation.Selected), (3, ItemRelation.Child) }, diffResult.ChangedIndexes);
        }
        
        [Fact]
        public void calculates_diff_from_level_2_to_level_1()
        {
            var node = CreateNodeOfLevel2();
            var otherNode = CreateNodeOfLevel1();
            var diffResult = TreeMenuDiff.Calculate(node, otherNode);
            Assert.Equal(new[] { 2, 4 }, diffResult.AddedIndexes);
            Assert.Equal(new[] { 3 }, diffResult.RemovedIndexes);
            Assert.Equal(new[] { (0, ItemRelation.Root), (1, ItemRelation.Selected), (2, ItemRelation.Child) }, diffResult.ChangedIndexes);
        }
        
        [Fact]
        public void calculates_diff_from_level_1_to_level_0()
        {
            var node = CreateNodeOfLevel1();
            var otherNode = CreateNodeOfLevel0();
            var diffResult = TreeMenuDiff.Calculate(node, otherNode);
            Assert.Equal(new[] { 1 }, diffResult.AddedIndexes);
            Assert.Equal(new[] { 0, 2, 3, 4 }, diffResult.RemovedIndexes);
            Assert.Equal(new[] { (1, ItemRelation.Child) }, diffResult.ChangedIndexes);
        }

        [Fact]
        public void calculates_diff_from_level_0_to_level_1_with_use_case_2()
        {
            var items = new List<SimpleLongItem> {
                new SimpleLongItem(TreeNode.ParentIdNone, 0),
                new SimpleLongItem(0, 1),
                new SimpleLongItem(0, 2),
                new SimpleLongItem(0, 3),
                new SimpleLongItem(0, 4),
                new SimpleLongItem(0, 5),
                new SimpleLongItem(1, 6),
                new SimpleLongItem(1, 7),
                new SimpleLongItem(3, 8),
                new SimpleLongItem(3, 9),
            };
            var node = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            var otherNode = node.FindChildNodeWithId(3);
            var diffResult = TreeMenuDiff.Calculate(node, otherNode);
            Assert.Equal(new[] { 0, 2, 3 }, diffResult.AddedIndexes);
            Assert.Equal(new[] { 0, 1, 3, 4 }, diffResult.RemovedIndexes);
            Assert.Equal(new[] { (2, ItemRelation.Selected) }, diffResult.ChangedIndexes);
        }
        
        [Fact]
        public void calculates_diff_from_level_1_to_level_0_with_use_case_2()
        {
            var items = new List<SimpleLongItem> {
                new SimpleLongItem(TreeNode.ParentIdNone, 0),
                new SimpleLongItem(0, 1),
                new SimpleLongItem(0, 2),
                new SimpleLongItem(0, 3),
                new SimpleLongItem(0, 4),
                new SimpleLongItem(0, 5),
                new SimpleLongItem(1, 6),
                new SimpleLongItem(1, 7),
                new SimpleLongItem(3, 8),
                new SimpleLongItem(3, 9),
            };
            var node = items.ToRootTreeNodes<SimpleLongItem, long>()[0];
            var otherNode = node.FindChildNodeWithId(3);
            var diffResult = TreeMenuDiff.Calculate(otherNode, node);
            Assert.Equal(new[] { 0, 1, 3, 4 }, diffResult.AddedIndexes);
            Assert.Equal(new[] { 0, 2, 3 }, diffResult.RemovedIndexes);
            Assert.Equal(new[] { (1, ItemRelation.Child) }, diffResult.ChangedIndexes);
        }
    }
}