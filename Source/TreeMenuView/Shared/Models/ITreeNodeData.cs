namespace TreeMenuView.Shared.Models
{
    public interface ITreeNodeData<out TKey>
    {
        TKey ParentId { get; }
        TKey Id { get; }
    }
}