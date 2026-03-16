namespace Festival.Domain
{
    /// <summary>
    /// Represents a staff member with system access credentials.
    /// </summary>
    public class Employee : IIdentifiable<long>
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Employee(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}