using System.Collections.Generic;
using TreeMenuView.Shared.Extensions;
using TreeMenuView.Shared.Models;
using Xunit;

namespace TreeMenuView.UnitTests
{
    public sealed class TreeMenuItemCollectionFixture
    {
        [Fact]
        public void returns_count_for_level_0()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel0();
            Assert.Equal(2, sut.Count);
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

        [Fact]
        public void returns_count_for_level_1()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel1();
            Assert.Equal(5, sut.Count);
        }

        private static TreeNode<SimpleLongItem, long> CreateNodeOfLevel1()
        {
            return CreateNodeOfLevel0().FindChildNodeWithId(1);
        }

        [Fact]
        public void returns_count_for_level_2()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel2();
            Assert.Equal(4, sut.Count);
        }

        private static TreeNode<SimpleLongItem, long> CreateNodeOfLevel2()
        {
            return CreateNodeOfLevel0().FindChildNodeWithId(4);
        }

        [Fact]
        public void returns_count_for_level_3()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel3();
            Assert.Equal(4, sut.Count);
        }

        private static TreeNode<SimpleLongItem, long> CreateNodeOfLevel3()
        {
            return CreateNodeOfLevel0().FindChildNodeWithId(8);
        }

        [Fact]
        public void returns_data_for_level_0()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel0();
            Assert.Equal(1, sut.GetItemData(0).Id);
            Assert.Equal(2, sut.GetItemData(1).Id);
        }
        
        [Fact]
        public void returns_data_for_level_1()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel1();
            Assert.Equal(0, sut.GetItemData(0).Id);
            Assert.Equal(1, sut.GetItemData(1).Id);
            Assert.Equal(3, sut.GetItemData(2).Id);
            Assert.Equal(4, sut.GetItemData(3).Id);
            Assert.Equal(5, sut.GetItemData(4).Id);
        }

        [Fact]
        public void returns_data_for_level_2()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel2();
            Assert.Equal(0, sut.GetItemData(0).Id);
            Assert.Equal(1, sut.GetItemData(1).Id);
            Assert.Equal(4, sut.GetItemData(2).Id);
            Assert.Equal(8, sut.GetItemData(3).Id);
        }
        
        [Fact]
        public void returns_data_for_level_3()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel3();
            Assert.Equal(0, sut.GetItemData(0).Id);
            Assert.Equal(1, sut.GetItemData(1).Id);
            Assert.Equal(4, sut.GetItemData(2).Id);
            Assert.Equal(8, sut.GetItemData(3).Id);
        }
        
        [Fact]
        public void returns_item_relation_for_level_0()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel0();
            Assert.Equal(ItemRelation.Child, sut.GetItemRelation(0));
            Assert.Equal(ItemRelation.Child, sut.GetItemRelation(1));
        }
        
        [Fact]
        public void returns_item_relation_for_level_1()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel1();
            Assert.Equal(ItemRelation.Root, sut.GetItemRelation(0));
            Assert.Equal(ItemRelation.Selected, sut.GetItemRelation(1));
            Assert.Equal(ItemRelation.Child, sut.GetItemRelation(2));
            Assert.Equal(ItemRelation.Child, sut.GetItemRelation(3));
            Assert.Equal(ItemRelation.Child, sut.GetItemRelation(4));
        }
        
        [Fact]
        public void returns_item_relation_for_level_2()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel2();
            Assert.Equal(ItemRelation.Root, sut.GetItemRelation(0));
            Assert.Equal(ItemRelation.Parent, sut.GetItemRelation(1));
            Assert.Equal(ItemRelation.Selected, sut.GetItemRelation(2));
            Assert.Equal(ItemRelation.Child, sut.GetItemRelation(3));
        }
        
        [Fact]
        public void returns_item_relation_for_level_3()
        {
            var sut = new TreeMenuItemCollection<SimpleLongItem, long>();
            sut.CurrentNode = CreateNodeOfLevel3();
            Assert.Equal(ItemRelation.Root, sut.GetItemRelation(0));
            Assert.Equal(ItemRelation.Parent, sut.GetItemRelation(1));
            Assert.Equal(ItemRelation.Parent, sut.GetItemRelation(2));
            Assert.Equal(ItemRelation.Selected, sut.GetItemRelation(3));
        }
    }
}