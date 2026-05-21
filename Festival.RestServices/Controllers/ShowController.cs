using Microsoft.AspNetCore.Mvc;
using Festival.Model;
using Festival.Persistence;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Festival.RestServices.Hubs;
using System.Threading.Tasks; // Required for async/await Task

namespace Festival.RestServices.Controllers
{
    /// <summary>
    /// REST Controller for managing Show resources.
    /// Provides endpoints for CRUD operations and filtering.
    /// Broadcasts real-time updates to connected clients via SignalR Hub.
    /// </summary>
    [ApiController]
    [Route("festival/shows")]
    public class ShowController : ControllerBase
    {
        private readonly IShowRepository _showRepository;
        private readonly IHubContext<ShowHub> _hubContext;

        public ShowController(IHubContext<ShowHub> hubContext) 
        {
            _showRepository = new ShowOrmRepository();
            _hubContext = hubContext;
        }

        // 1. GET: festival/shows
        [HttpGet]
        public ActionResult<IEnumerable<Show>> GetAll()
        {
            Console.WriteLine("[REST] Fetching all shows...");
            return Ok(_showRepository.FindAll());
        }

        // 2. GET: festival/shows/{id}
        [HttpGet("{id:long}")]
        public ActionResult<Show> GetById(long id)
        {
            Console.WriteLine($"[REST] Fetching show with ID: {id}");
            var show = _showRepository.FindOne(id);
            if (show == null)
            {
                return NotFound($"Show with ID {id} not found.");
            }
            return Ok(show);
        }

        // 3. GET: festival/shows/filter?date=2026-07-15
        [HttpGet("filter")]
        public ActionResult<IEnumerable<Show>> GetByDate([FromQuery] DateTime date)
        {
            Console.WriteLine($"[REST] Filtering shows by date: {date.ToShortDateString()}");
            return Ok(_showRepository.FindByDate(date));
        }

        // 4. POST: festival/shows
        // MODIFIED: Added 'async Task<...>' to allow 'await' inside the method
        [HttpPost]
        public async Task<ActionResult<Show>> Create([FromBody] Show show)
        {
            Console.WriteLine("[REST] Creating new show...");
            try
            {
                _showRepository.Add(show);
                
                // Broadcast to all connected clients that a new show was created
                await _hubContext.Clients.All.SendAsync("ReceiveShowUpdate", "Created");
                
                return CreatedAtAction(nameof(GetById), new { id = show.Id }, show);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 5. PUT: festival/shows/{id}
        // MODIFIED: Added 'async Task<...>'
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] Show show)
        {
            Console.WriteLine($"[REST] Updating show with ID: {id}");
    
            if (id != show.Id)
            {
                return BadRequest("ID mismatch between URL and body.");
            }

            try
            {
                var existingShow = _showRepository.FindOne(id); 

                if (existingShow == null)
                {
                    return NotFound($"Show with ID {id} not found."); 
                }

                _showRepository.Update(show);
                
                // Broadcast the update event
                await _hubContext.Clients.All.SendAsync("ReceiveShowUpdate", "Updated");
                
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 6. DELETE: festival/shows/{id}
        // MODIFIED: Added 'async Task<...>'
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            Console.WriteLine($"[REST] Deleting show with ID: {id}");
            
            var existing = _showRepository.FindOne(id);
            if (existing == null)
            {
                return NotFound();
            }

            _showRepository.Delete(id);
            
            // Broadcast the deletion event (Changed to "Deleted" for accuracy)
            await _hubContext.Clients.All.SendAsync("ReceiveShowUpdate", "Deleted");
            
            return NoContent();
        }
    }
}