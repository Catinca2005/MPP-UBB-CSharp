using System.Collections.Generic;
using Festival.Model;

namespace Festival.Persistence
{
    public interface ITicketRepository : IRepository<long, Ticket>
    {
        IEnumerable<Ticket> FindAllByShow(long showId);
    }
}