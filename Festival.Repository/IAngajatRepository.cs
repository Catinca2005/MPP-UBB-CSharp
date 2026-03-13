using Festival.Domain;

namespace Festival.Repository
{
    public interface IAngajatRepository : IRepository<long, Angajat>
    {
        Angajat FindByUsernameAndPassword(string username, string password);
    }
}