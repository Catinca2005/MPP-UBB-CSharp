namespace Festival.Model
{
    /// <summary>
    /// Represents a staff member with system access credentials.
    /// </summary>
    public class Employee : IIdentifiable<long>
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Required by Entity Framework Core for object materialization
        protected Employee() { }
        
        public Employee(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}