namespace TreeMenuView.Shared.Models
{
    public interface ITreeMenuCell<TData>
    {
        TData Data { set; }
        ItemRelation Relation { set; }
    }
}