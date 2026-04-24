using System;
using Festival.Model;
using Festival.Networking.Protobuf; 

namespace Festival.Networking
{
    /// <summary>
    /// Utility class for mapping between Domain Models and Protobuf DTOs.
    /// Ensures clean separation between business logic entities and network transfer objects.
    /// </summary>
    public static class ProtoUtils
    {
        public static Show GetShow(ShowProto proto)
        {
            DateTime date = DateTime.Parse(proto.Date);
            TimeSpan time = TimeSpan.Parse(proto.Time);
            return new Show(proto.ArtistId, date, time, proto.Location, proto.AvailableSeats, proto.SoldSeats)
            {
                Id = proto.Id
            };
        }

        public static ShowProto GetShowProto(Show show)
        {
            return new ShowProto
            {
                Id = show.Id,
                ArtistId = show.ArtistId,
                Date = show.Date.ToString("yyyy-MM-dd"),
                Time = show.Time.ToString(@"hh\:mm\:ss"),
                Location = show.Location,
                AvailableSeats = show.AvailableSeats,
                SoldSeats = show.SoldSeats
            };
        }

        public static Employee GetEmployee(EmployeeProto proto)
        {
            return new Employee(proto.Username, proto.Password);
        }

        public static EmployeeProto GetEmployeeProto(Employee employee)
        {
            return new EmployeeProto
            {
                Username = employee.Username,
                Password = employee.Password
            };
        }

        public static Ticket GetTicket(TicketProto proto)
        {
            return new Ticket(proto.ShowId, proto.BuyerName, proto.NumberOfSeats)
            {
                Id = proto.Id
            };
        }

        public static TicketProto GetTicketProto(Ticket ticket)
        {
            return new TicketProto
            {
                Id = ticket.Id,
                ShowId = ticket.ShowId,
                BuyerName = ticket.BuyerName,
                NumberOfSeats = ticket.NumberOfSeats
            };
        }
    }
}