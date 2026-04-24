using Festival.Model;

namespace Festival.Services
{
    /// <summary>
    /// Contract for the client-side observer. 
    /// Enables the server to push real-time live updates to connected clients.
    /// </summary>
    public interface IFestivalObserver
    {
        /// <summary>
        /// Triggered by the server when a ticket is sold.
        /// </summary>
        /// <param name="updatedShow">The show entity containing the updated seat inventory.</param>
        void TicketSold(Show updatedShow);
    }
}