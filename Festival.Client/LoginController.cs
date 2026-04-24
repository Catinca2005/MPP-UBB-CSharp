using System;
using Festival.Model;
using Festival.Services;

namespace Festival.Controller
{
    /// <summary>
    /// Manages the authentication workflow between the Login UI and the Server.
    /// </summary>
    public class LoginController
    {
        private readonly IFestivalServices _server;
        private readonly MainController _mainController;

        public LoginController(IFestivalServices server, MainController mainController)
        {
            _server = server;
            _mainController = mainController;
        }

        /// <summary>
        /// Authenticates the user credentials against the remote server.
        /// </summary>
        public Employee HandleLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Credentials cannot be empty.");
            }

            Employee employee = new Employee(username, password);
            
            // Attempt to login. We pass the MainController as the IFestivalObserver to receive live updates.
            _server.Login(employee, _mainController);
            
            // If successful, establish the current session context for the main application
            _mainController.SetCurrentUser(employee);
            
            return employee;
        }
    }
}