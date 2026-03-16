using System.Collections.Generic;
using Festival.Domain;

namespace Festival.Repository
{
    public interface ITicketRepository : IRepository<long, Ticket>
    {
        IEnumerable<Ticket> FindAllByShow(long showId);
    }
}