namespace TreeMenuView.Shared.Models
{
    public interface ITreeMenuCell
    {
        string Title { set; }
        ItemRelation Relation { set; }
    }
}