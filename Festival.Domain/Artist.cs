namespace Festival.Domain
{
    public class Artist : IIdentifiable<long>
    {
        public long Id { get; set; }
        public string Nume { get; set; }
        public Artist(string nume) { Nume = nume; }
    }
}