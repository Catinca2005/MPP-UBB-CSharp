using System.Collections.Generic;
using Festival.Model;

namespace Festival.Persistence
{
    public interface IRepository<ID, T> where T : IIdentifiable<ID>
    {
        void Add(T elem);
        void Update(T elem);
        void Delete(ID id);
        T FindOne(ID id);
        IEnumerable<T> FindAll();
    }
}