namespace Festival.Domain
{
    public class Angajat : IIdentifiable<long>
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Angajat(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}