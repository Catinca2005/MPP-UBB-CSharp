using System;
using System.Collections.Generic;
using System.Linq;
using Festival.Model;

namespace Festival.Persistence
{
    /// <summary>
    /// Entity Framework Core implementation for Show data operations.
    /// Safely handles database transactions using thread-scoped contexts.
    /// </summary>
    public class ShowOrmRepository : IShowRepository
    {
        public void Add(Show elem)
        {
            using var context = new FestivalDbContext();
            context.Shows.Add(elem);
            context.SaveChanges();
        }

        public void Update(Show elem)
        {
            using var context = new FestivalDbContext();
            context.Shows.Update(elem);
            context.SaveChanges();
        }

        public void Delete(long id)
        {
            using var context = new FestivalDbContext();
            var show = context.Shows.Find(id);
            if (show != null)
            {
                context.Shows.Remove(show);
                context.SaveChanges();
            }
        }

        public Show FindOne(long id)
        {
            using var context = new FestivalDbContext();
            return context.Shows.Find(id);
        }

        public IEnumerable<Show> FindAll()
        {
            using var context = new FestivalDbContext();
            return context.Shows.ToList();
        }

        public IEnumerable<Show> FindByDate(DateTime date)
        {
            using var context = new FestivalDbContext();
            
            // Compares the date segment only, translating natively to SQLite date functions
            return context.Shows
                .Where(s => s.Date.Date == date.Date)
                .ToList();
        }
    }
}