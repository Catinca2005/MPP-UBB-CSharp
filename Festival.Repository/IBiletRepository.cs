using System.Collections.Generic;
using Festival.Domain;

namespace Festival.Repository
{
    public interface IBiletRepository : IRepository<long, Bilet>
    {
        IEnumerable<Bilet> FindAllBySpectacol(long spectacolId);
    }
}