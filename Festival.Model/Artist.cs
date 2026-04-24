namespace Festival.Model
{
    /// <summary>
    /// Represents a performer participating in the festival.
    /// </summary>
    public class Artist : IIdentifiable<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Artist(string name) => Name = name;
    }
}