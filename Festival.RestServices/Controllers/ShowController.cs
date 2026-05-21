using Microsoft.AspNetCore.Mvc;
using Festival.Model;
using Festival.Persistence;
using System;
using System.Collections.Generic;

namespace Festival.RestServices.Controllers
{
    /// <summary>
    /// REST Controller for managing Show resources.
    /// Provides endpoints for CRUD operations and filtering.
    /// </summary>
    [ApiController]
    [Route("festival/shows")]
    public class ShowController : ControllerBase
    {
        private readonly IShowRepository _showRepository;

        public ShowController()
        {
            // We instantiate the ORM repository directly.
            _showRepository = new ShowOrmRepository();
        }

        // 1. GET: festival/shows (Get all shows)
        [HttpGet]
        public ActionResult<IEnumerable<Show>> GetAll()
        {
            Console.WriteLine("[REST] Fetching all shows...");
            return Ok(_showRepository.FindAll());
        }

        // 2. GET: festival/shows/{id} (Get show by ID)
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

        // 3. GET: festival/shows/filter?date=2026-07-15 (Filtering)
        [HttpGet("filter")]
        public ActionResult<IEnumerable<Show>> GetByDate([FromQuery] DateTime date)
        {
            Console.WriteLine($"[REST] Filtering shows by date: {date.ToShortDateString()}");
            return Ok(_showRepository.FindByDate(date));
        }

        // 4. POST: festival/shows (Create new show)
        [HttpPost]
        public ActionResult<Show> Create([FromBody] Show show)
        {
            Console.WriteLine("[REST] Creating new show...");
            try
            {
                _showRepository.Add(show);
                // Requirement: Return the created object which now includes the generated ID.
                // CreatedAtAction returns a 201 Created status and a Location header.
                return CreatedAtAction(nameof(GetById), new { id = show.Id }, show);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 5. PUT: festival/shows/{id} (Update existing show)
        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] Show show)
        {
            Console.WriteLine($"[REST] Updating show with ID: {id}");
    
            // Validation 1: Check if the ID from the URL matches the ID from the JSON body
            if (id != show.Id)
            {
                return BadRequest("ID mismatch between URL and body.");
            }

            try
            {
                // Validation 2: Explicitly search for the existing record before attempting an update
                
                var existingShow = _showRepository.FindOne(id); 

                if (existingShow == null)
                {
                    // Return 404 explicitly if the item does not exist in the database
                    return NotFound($"Show with ID {id} not found."); 
                }

                // The entity exists, proceed with the update operation
                _showRepository.Update(show);
                return NoContent(); // 204 No Content is standard for successful updates
            }
            catch (Exception ex)
            {
                // Any other database constraints or connection issues will still return a 400 Bad Request
                return BadRequest(ex.Message);
            }
        }

        // 6. DELETE: festival/shows/{id} (Delete show)
        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            Console.WriteLine($"[REST] Deleting show with ID: {id}");
            var existing = _showRepository.FindOne(id);
            if (existing == null)
            {
                return NotFound();
            }

            _showRepository.Delete(id);
            return NoContent();
        }
    }
}