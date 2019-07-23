using System.Collections.Generic;
using TreeMenuView.Shared.Models;

namespace TreeMenuSample.Shared
{
    public sealed class Category : ITreeNodeData<long>
    {
        public static IEnumerable<Category> CreateSamples()
        {
            return new List<Category> {
                new Category(TreeNode.ParentIdNone, 0, "All categories"),
                new Category(0, 1, "Development"),
                new Category(0, 2, "Recipes"),
                new Category(0, 3, "Sport"),
                new Category(0, 4, "Music"),
                new Category(0, 5, "Cars"),
                new Category(1, 6, "Android Development"),
                new Category(1, 7, "iOS Development"),
                new Category(3, 8, "Football"),
                new Category(3, 9, "Formula 1")
            };
        }
        
        public Category(long parentId, long id, string title)
        {
            ParentId = parentId;
            Id = id;
            Title = title;
        }

        public long ParentId { get; }
        public long Id { get; }
        public string Title { get; }
    }
}