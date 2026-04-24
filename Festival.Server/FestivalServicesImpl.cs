using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Festival.Model;
using Festival.Persistence;
using Festival.Services;

namespace Festival.Server
{
    /// <summary>
    /// Core implementation of the festival business logic on the server side.
    /// Manages concurrent client sessions and coordinates database transactions.
    /// </summary>
    public class FestivalServicesImpl : IFestivalServices
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IShowRepository _showRepository;
        private readonly ITicketRepository _ticketRepository;
        
        // Thread-safe dictionary to keep track of logged-in clients and their active observer network instances
        private readonly ConcurrentDictionary<string, IFestivalObserver> _loggedClients;

        public FestivalServicesImpl(IEmployeeRepository employeeRepo, IArtistRepository artistRepo, IShowRepository showRepo, ITicketRepository ticketRepo)
        {
            _employeeRepository = employeeRepo;
            _artistRepository = artistRepo;
            _showRepository = showRepo;
            _ticketRepository = ticketRepo;
            _loggedClients = new ConcurrentDictionary<string, IFestivalObserver>();
        }

        public void Login(Employee employee, IFestivalObserver client)
        {
            Employee validUser = _employeeRepository.FindByUsernameAndPassword(employee.Username, employee.Password);
            
            if (validUser != null)
            {
                if (_loggedClients.ContainsKey(employee.Username))
                {
                    throw new FestivalException("Authentication rejected: User is already logged in from another instance.");
                }
                
                // Register the client observer to receive live updates
                _loggedClients[employee.Username] = client;
            }
            else
            {
                throw new FestivalException("Authentication failed: Invalid credentials.");
            }
        }

        public void Logout(Employee employee, IFestivalObserver client)
        {
            // Unregister the client observer safely
            bool isRemoved = _loggedClients.TryRemove(employee.Username, out _);
            
            if (!isRemoved)
            {
                throw new FestivalException("Logout failed: User is not currently logged in.");
            }
        }

        public IEnumerable<Show> GetAllShows()
        {
            return _showRepository.FindAll();
        }

        public IEnumerable<Show> GetShowsByDate(DateTime date)
        {
            return _showRepository.FindByDate(date);
        }

        public void BuyTicket(Ticket ticket)
        {
            Show show = _showRepository.FindOne(ticket.ShowId);
            if (show == null)
            {
                throw new FestivalException("Transaction failed: Show not found in the inventory.");
            }

            if (show.AvailableSeats < ticket.NumberOfSeats)
            {
                throw new FestivalException("Transaction failed: Insufficient available seats.");
            }

            // Execute the transaction
            show.SellSeats(ticket.NumberOfSeats);
            _showRepository.Update(show);
            _ticketRepository.Add(ticket);

            // Broadcast the inventory update to all other connected clients
            NotifyClientsTicketSold(show);
        }

        /// <summary>
        /// Pushes an asynchronous event notification to all active client observers.
        /// Uses Task.Run to prevent a slow client from blocking the server thread.
        /// </summary>
        private void NotifyClientsTicketSold(Show updatedShow)
        {
            foreach (var client in _loggedClients.Values)
            {
                Task.Run(() => 
                {
                    try
                    {
                        client.TicketSold(updatedShow);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[WARNING] Failed to push live update to a client: {ex.Message}");
                    }
                });
            }
        }
    }
}