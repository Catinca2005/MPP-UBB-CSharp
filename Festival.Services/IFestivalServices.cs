using System;
using System.Collections.Generic;
using Festival.Model;

namespace Festival.Services
{
    /// <summary>
    /// Core business logic contract.
    /// Acts as the primary interface for client-to-server RPC (Remote Procedure Calls).
    /// </summary>
    public interface IFestivalServices
    {
        /// <summary>
        /// Authenticates an employee and registers their client observer for live updates.
        /// </summary>
        /// <param name="employee">The employee credentials.</param>
        /// <param name="client">The client observer instance.</param>
        /// <exception cref="FestivalException">Thrown if authentication fails or user is already logged in.</exception>
        void Login(Employee employee, IFestivalObserver client);

        /// <summary>
        /// Safely disconnects the employee and unregisters their observer.
        /// </summary>
        void Logout(Employee employee, IFestivalObserver client);

        /// <summary>
        /// Retrieves the complete inventory of available shows.
        /// </summary>
        IEnumerable<Show> GetAllShows();

        /// <summary>
        /// Filters the shows scheduled for a specific date.
        /// </summary>
        IEnumerable<Show> GetShowsByDate(DateTime date);

        /// <summary>
        /// Processes a ticket purchase transaction and triggers live updates to all other observers.
        /// </summary>
        /// <param name="ticket">The ticket entity containing purchase details.</param>
        /// <exception cref="FestivalException">Thrown if validation fails or seats are insufficient.</exception>
        void BuyTicket(Ticket ticket);
    }
}