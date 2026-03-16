namespace Festival.Domain
{
    /// <summary>
    /// Represents a purchase record linking a buyer to a specific show.
    /// </summary>
    public class Ticket : IIdentifiable<long>
    {
        public long Id { get; set; }
        public long ShowId { get; set; }
        public string BuyerName { get; set; }
        public int NumberOfSeats { get; set; }

        public Ticket(long showId, string buyerName, int numberOfSeats)
        {
            ShowId = showId;
            BuyerName = buyerName;
            NumberOfSeats = numberOfSeats;
        }
    }
}