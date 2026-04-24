using Festival.Model;

namespace Festival.Persistence
{
    public interface IEmployeeRepository : IRepository<long, Employee>
    {
        Employee FindByUsernameAndPassword(string username, string password);
    }
}