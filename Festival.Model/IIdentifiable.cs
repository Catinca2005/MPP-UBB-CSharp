namespace Festival.Model
{
    public interface IIdentifiable<TId>
    {
        TId Id { get; set; }
    }
}