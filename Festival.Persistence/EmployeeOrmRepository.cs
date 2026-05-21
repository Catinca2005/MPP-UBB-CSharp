using System.Collections.Generic;
using System.Linq;
using Festival.Model;

namespace Festival.Persistence
{
    /// <summary>
    /// Entity Framework Core implementation for Employee data operations.
    /// Safely handles database transactions using thread-scoped contexts.
    /// </summary>
    public class EmployeeOrmRepository : IEmployeeRepository
    {
        public void Add(Employee elem)
        {
            using var context = new FestivalDbContext();
            context.Employees.Add(elem);
            context.SaveChanges();
        }

        public void Update(Employee elem)
        {
            using var context = new FestivalDbContext();
            context.Employees.Update(elem);
            context.SaveChanges();
        }

        public void Delete(long id)
        {
            using var context = new FestivalDbContext();
            var employee = context.Employees.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
        }

        public Employee FindOne(long id)
        {
            using var context = new FestivalDbContext();
            return context.Employees.Find(id);
        }

        public IEnumerable<Employee> FindAll()
        {
            using var context = new FestivalDbContext();
            return context.Employees.ToList();
        }

        public Employee FindByUsernameAndPassword(string username, string password)
        {
            // Encode the plaintext password provided by the user to match the database format
            string encodedPassword = SecurityUtils.Encode(password);

            using var context = new FestivalDbContext();
            
            // Translates into a secure, parameterized SQL SELECT query (prevents SQL Injection)
            // It uses the encoded password for the search criteria
            return context.Employees
                .FirstOrDefault(e => e.Username == username && e.Password == encodedPassword);
        }
    }
}