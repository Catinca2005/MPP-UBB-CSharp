namespace Festival.Domain
{
    public interface IIdentifiable<TId>
    {
        TId Id { get; set; }
    }
}