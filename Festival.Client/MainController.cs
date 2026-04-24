using System;
using System.Collections.Generic;
using Festival.Model;
using Festival.Services;

namespace Festival.Controller
{
    /// <summary>
    /// Orchestrates the primary business flows of the application.
    /// Implements IFestivalObserver to handle asynchronous live updates pushed from the server.
    /// </summary>
    public class MainController : IFestivalObserver
    {
        private readonly IFestivalServices _server;
        private Employee _currentUser;

        // Event exposed to the View (UI) to subscribe to real-time inventory updates
        public event EventHandler<Show> OnTicketSold;

        public MainController(IFestivalServices server)
        {
            _server = server;
        }

        public void SetCurrentUser(Employee user)
        {
            _currentUser = user;
        }

        public Employee GetCurrentUser()
        {
            return _currentUser;
        }

        public IEnumerable<Show> HandleGetAllShows()
        {
            return _server.GetAllShows();
        }

        public IEnumerable<Show> HandleSearch(DateTime date)
        {
            return _server.GetShowsByDate(date);
        }

        /// <summary>
        /// Processes a ticket purchase and relays it to the server.
        /// </summary>
        public void HandleBuyTicket(long showId, string buyerName, string quantityStr)
        {
            if (string.IsNullOrWhiteSpace(buyerName)) 
                throw new Exception("Buyer name is required.");
                
            if (!int.TryParse(quantityStr, out int quantity) || quantity <= 0) 
                throw new Exception("Invalid seat quantity. Must be a positive integer.");

            Ticket ticket = new Ticket(showId, buyerName, quantity);
            _server.BuyTicket(ticket);
        }

        /// <summary>
        /// Safely terminates the active server session.
        /// </summary>
        public void HandleLogout()
        {
            if (_currentUser != null)
            {
                _server.Logout(_currentUser, this);
                _currentUser = null;
            }
        }

        // --- IFestivalObserver Implementation ---

        /// <summary>
        /// Callback method triggered by the background network thread when the server pushes an update.
        /// </summary>
        public void TicketSold(Show updatedShow)
        {
            OnTicketSold?.Invoke(this, updatedShow);
        }
    }
}