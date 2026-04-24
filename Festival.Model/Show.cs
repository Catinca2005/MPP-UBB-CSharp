namespace Festival.Model
{
    using System;

    /// <summary>
    /// Represents a scheduled event including time, location, and seat inventory.
    /// </summary>
    public class Show : IIdentifiable<long>
    {
        public long Id { get; set; }
        public long ArtistId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Location { get; set; }
        public int AvailableSeats { get; set; }
        public int SoldSeats { get; set; }

        public Show(long artistId, DateTime date, TimeSpan time, string location, int available, int sold)
        {
            ArtistId = artistId;
            Date = date;
            Time = time;
            Location = location;
            AvailableSeats = available;
            SoldSeats = sold;
        }

        public void SellSeats(int quantity)
        {
            AvailableSeats -= quantity;
            SoldSeats += quantity;
        }
    }
}