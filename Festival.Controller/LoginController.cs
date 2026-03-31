using System;
using Festival.Domain;
using Festival.Service;

namespace Festival.Controller
{
    public class LoginController
    {
        private readonly FestivalService _service;

        public LoginController(FestivalService service)
        {
            _service = service;
        }

        /// <summary>
        /// Handles the login logic. Returns the Employee if successful.
        /// </summary>
        public Employee HandleLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Username and password cannot be empty.");
            }

            // The service handles the actual DB check and logic
            return _service.Login(username, password);
        }
    }
}