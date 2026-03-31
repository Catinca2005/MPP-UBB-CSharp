using System;
using System.Collections.Generic;
using Festival.Domain;
using Festival.Service;

namespace Festival.Controller
{
    public class MainController
    {
        private readonly FestivalService _service;

        public MainController(FestivalService service)
        {
            _service = service;
        }

        public IEnumerable<Show> HandleGetAllShows()
        {
            return _service.GetAllShows();
        }

        public IEnumerable<Show> HandleSearch(DateTime date)
        {
            return _service.GetShowsByDate(date);
        }

        public void HandleBuyTicket(long showId, string buyer, string quantityStr)
        {
            if (string.IsNullOrWhiteSpace(buyer)) throw new Exception("Buyer name is required.");
            if (!int.TryParse(quantityStr, out int quantity)) throw new Exception("Invalid seat quantity.");

            _service.BuyTicket(showId, buyer, quantity);
        }
    }
}