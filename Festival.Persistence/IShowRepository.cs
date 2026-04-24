using System;
using System.Collections.Generic;
using Festival.Model;

namespace Festival.Persistence
{
    public interface IShowRepository : IRepository<long, Show>
    {
        IEnumerable<Show> FindByDate(DateTime date);
    }
}