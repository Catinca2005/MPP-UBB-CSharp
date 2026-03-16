using Festival.Domain;

namespace Festival.Repository
{
    public interface IEmployeeRepository : IRepository<long, Employee>
    {
        Employee FindByUsernameAndPassword(string username, string password);
    }
}