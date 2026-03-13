using System;
using System.Collections.Generic;
using Festival.Domain;

namespace Festival.Repository
{
    public interface ISpectacolRepository : IRepository<long, Spectacol>
    {
        IEnumerable<Spectacol> FindByDate(DateTime date);
    }
}