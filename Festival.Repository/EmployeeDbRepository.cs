using System;
using System.Collections.Generic;
using System.Data;
using Festival.Domain;
using log4net;

namespace Festival.Repository
{
    /// <summary>
    /// Data Access Object for Employee entities using SQLite and ADO.NET.
    /// Provides persistence logic and specialized authentication queries.
    /// </summary>
    public class EmployeeDbRepository : IEmployeeRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EmployeeDbRepository));

        public Employee FindByUsernameAndPassword(string username, string password)
        {
            Log.InfoFormat("Entering FindByUsernameAndPassword with username: {0}", username);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id FROM employees WHERE username=@user AND password=@pass";

                var paramUser = comm.CreateParameter();
                paramUser.ParameterName = "@user";
                paramUser.Value = username;
                comm.Parameters.Add(paramUser);

                var paramPass = comm.CreateParameter();
                paramPass.ParameterName = "@pass";
                paramPass.Value = password;
                comm.Parameters.Add(paramPass);

                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        long id = reader.GetInt64(0);
                        Employee employee = new Employee(username, password) { Id = id };
                        Log.InfoFormat("Exiting FindByUsernameAndPassword - Employee found with ID: {0}", id);
                        return employee;
                    }
                }
            }
            Log.Info("Exiting FindByUsernameAndPassword - No matching employee found");
            return null;
        }

        public void Add(Employee entity)
        {
            Log.InfoFormat("Entering Add for employee: {0}", entity.Username);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO employees (username, password) VALUES (@user, @pass)";

                var p1 = comm.CreateParameter(); p1.ParameterName = "@user"; p1.Value = entity.Username; comm.Parameters.Add(p1);
                var p2 = comm.CreateParameter(); p2.ParameterName = "@pass"; p2.Value = entity.Password; comm.Parameters.Add(p2);

                comm.ExecuteNonQuery();
            }
            Log.Info("Exiting Add");
        }

        public void Update(Employee entity)
        {
            Log.InfoFormat("Entering Update for employee ID: {0}", entity.Id);
            IDbConnection con = DbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "UPDATE employees SET username=@user, password=@pass WHERE id=@id";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@user"; p1.Value = entity.Username; comm.Parameters.Add(p1);
                var p2 = comm.CreateParameter(); p2.ParameterName = "@pass"; p2.Value = entity.Password; comm.Parameters.Add(p2);
                var p3 = comm.CreateParameter(); p3.ParameterName = "@id"; p3.Value = entity.Id; comm.Parameters.Add(p3);
                comm.ExecuteNonQuery();
            }
        }

        public void Delete(long id)
        {
            Log.InfoFormat("Entering Delete for employee ID: {0}", id);
            IDbConnection con = DbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM employees WHERE id=@id";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@id"; p1.Value = id; comm.Parameters.Add(p1);
                comm.ExecuteNonQuery();
            }
        }

        public Employee FindOne(long id)
        {
            Log.InfoFormat("Entering FindOne for employee ID: {0}", id);
            IDbConnection con = DbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT username, password FROM employees WHERE id=@id";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@id"; p1.Value = id; comm.Parameters.Add(p1);
                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employee(reader.GetString(0), reader.GetString(1)) { Id = id };
                    }
                }
            }
            return null;
        }

        public IEnumerable<Employee> FindAll()
        {
            Log.Info("Entering FindAll employees");
            IDbConnection con = DbUtils.GetConnection();
            IList<Employee> employees = new List<Employee>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, username, password FROM employees";
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee(reader.GetString(1), reader.GetString(2)) { Id = reader.GetInt64(0) });
                    }
                }
            }
            return employees;
        }
    }
}