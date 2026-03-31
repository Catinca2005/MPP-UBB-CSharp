using System;
using System.Collections.Generic;
using Festival.Domain;
using Festival.Repository;

namespace Festival.Service
{
    public class FestivalService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IArtistRepository _artistRepo;
        private readonly IShowRepository _showRepo;
        private readonly ITicketRepository _ticketRepo;

        public FestivalService(IEmployeeRepository employeeRepo, IArtistRepository artistRepo, 
                               IShowRepository showRepo, ITicketRepository ticketRepo)
        {
            _employeeRepo = employeeRepo;
            _artistRepo = artistRepo;
            _showRepo = showRepo;
            _ticketRepo = ticketRepo;
        }

        // Requirement 1: Login logic
        public Employee Login(string username, string password)
        {
            Employee emp = _employeeRepo.FindByUsernameAndPassword(username, password);
            if (emp == null)
            {
                throw new Exception("Authentication failed: Invalid username or password.");
            }
            return emp;
        }

        // Requirement 1: Get all shows for the main table
        public IEnumerable<Show> GetAllShows()
        {
            return _showRepo.FindAll();
        }

        // Requirement 2: Search artists/shows by date
        public IEnumerable<Show> GetShowsByDate(DateTime date)
        {
            return _showRepo.FindByDate(date);
        }

        // Requirement 3: Purchase logic
        public void BuyTicket(long showId, string buyerName, int quantity)
        {
            Show show = _showRepo.FindOne(showId);
            if (show == null) throw new Exception("Show not found.");

            if (show.AvailableSeats < quantity)
            {
                throw new Exception("Not enough seats available!");
            }

            // Update show inventory
            show.SellSeats(quantity);
            _showRepo.Update(show);

            // Record the ticket
            Ticket ticket = new Ticket(showId, buyerName, quantity);
            _ticketRepo.Add(ticket);
        }

        // Requirement 4: Modify existing ticket (Increase seats)
        public void UpdateTicketSeats(long ticketId, int additionalSeats)
        {
            Ticket ticket = _ticketRepo.FindOne(ticketId);
            if (ticket == null) throw new Exception("Ticket not found.");

            Show show = _showRepo.FindOne(ticket.ShowId);
            if (show.AvailableSeats < additionalSeats)
            {
                throw new Exception("Not enough seats to expand this booking.");
            }

            // Update Show
            show.SellSeats(additionalSeats);
            _showRepo.Update(show);

            // Update Ticket
            ticket.NumberOfSeats += additionalSeats;
            _ticketRepo.Update(ticket);
        }
    }
}