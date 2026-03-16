using System;
using System.Collections.Generic;
using Festival.Domain;

namespace Festival.Repository
{
    public interface IShowRepository : IRepository<long, Show>
    {
        IEnumerable<Show> FindByDate(DateTime date);
    }
}