using Microsoft.AspNetCore.Mvc;
using RadonAPI.Data;
using RadonAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RadonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly MongoDBContext _context;

        public LogsController(MongoDBContext context)
        {
            _context = context;
        }

        // GET: api/<LogsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var logs = await _context.Logs.Find(_ => true).ToListAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the database operation
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving logs: {ex.Message}");
            }
        }


        // GET api/<LogsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> Get(string id)
        {
            try
            {
                var log = await _context.Logs.Find(l => l.Id == id).SingleOrDefaultAsync();

                if (log == null)
                {
                    return NotFound();
                }

                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving log {id}: {ex.Message}");
            }
        }

        // POST api/<LogsController>
        [HttpPost]
        public async Task<ActionResult<Log>> Create(Log log)
        {
            try
            {
                await _context.Logs.InsertOneAsync(log);

                return CreatedAtAction(nameof(Get), new { id = log.Id }, log);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while inserting a log: {ex.Message}");
            }
            
        }

        // PUT api/<LogsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Log logInput)
        {
            try
            {
                if (logInput == null || string.IsNullOrEmpty(id))
                {
                    return BadRequest("Invalid log or log ID");
                }

                var existingLog = await _context.Logs.Find(l => l.Id == id).SingleOrDefaultAsync();

                if (existingLog == null)
                {
                    return NotFound();
                }

                await _context.Logs.ReplaceOneAsync(l => l.Id == id, logInput);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while modifying log {id}: {ex.Message}");
            }
        }

        // DELETE api/<LogsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var log = await _context.Logs.Find(l => l.Id == id).SingleOrDefaultAsync();

                if (log == null)
                {
                    return NotFound();
                }

                await _context.Logs.DeleteOneAsync(l => l.Id == id);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting log {id}: {ex.Message}");
            }
        }
    }
}
