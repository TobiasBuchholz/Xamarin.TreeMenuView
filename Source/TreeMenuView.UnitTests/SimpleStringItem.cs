using TreeMenuView.Shared.Models;

namespace TreeMenuView.UnitTests
{
    public class SimpleStringItem : ITreeNodeData<string>
    {
        public SimpleStringItem(string parentId, string id)
        {
            ParentId = parentId;
            Id = id;
            Title = "Foobar";
        }
        
        public override bool Equals(object obj)
        {
            if(obj is SimpleStringItem other) {
                return Equals(other);
            }
            return false;
        }
        
        private bool Equals(SimpleStringItem other)
        {
            return (ParentId, Id, Title).Equals((other.ParentId, other.Id, other.Title));
        }

        public override int GetHashCode()
        {
            return (ParentId, Id, Title).GetHashCode();
        }

        public string ParentId { get; }
        public string Id { get; }
        public string Title { get; }
    }
}