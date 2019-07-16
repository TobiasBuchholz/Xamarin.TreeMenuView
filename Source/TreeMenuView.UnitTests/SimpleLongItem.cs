using TreeMenuView.Shared.Models;

namespace TreeMenuView.UnitTests
{
    public class SimpleLongItem : ITreeNodeData<long>
    {
        public SimpleLongItem(long parentId, long id)
        {
            ParentId = parentId;
            Id = id;
            Title = "Foobar";
        }

        public override bool Equals(object obj)
        {
            if(obj is SimpleLongItem other) {
                return Equals(other);
            }
            return false;
        }

        private bool Equals(SimpleLongItem other)
        {
            return (ParentId, Id, Title).Equals((other.ParentId, other.Id, other.Title));
        }

        public override int GetHashCode()
        {
            return (ParentId, Id, Title).GetHashCode();
        }

        public long ParentId { get; }
        public long Id { get; }
        public string Title { get; }
    }
}