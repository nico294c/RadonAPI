using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RadonAPI.Data;
using RadonAPI.Models;

namespace RadonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataloggersController : ControllerBase
    {
        private readonly MongoDBContext _context;

        public DataloggersController(MongoDBContext context)
        {
            _context = context;
        }

        // GET: api/<DataloggersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var dataloggers = await _context.Dataloggers.Find(_ => true).ToListAsync();
                return Ok(dataloggers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving dataloggers: {ex.Message}");
            }
        }

        // GET api/<DataloggersController>/5
        [HttpGet("{serialnumber}")]
        public async Task<ActionResult<Datalogger>> Get(string serialnumber)
        {
            try
            {
                var datalogger = await _context.Dataloggers.Find(d => d.Serialnumber == serialnumber).SingleOrDefaultAsync();

                if (datalogger == null)
                {
                    return NotFound();
                }

                return Ok(datalogger);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving datalogger {serialnumber}: {ex.Message}");
            }
        }

        // POST api/<DataloggersController>
        [HttpPost]
        public async Task<ActionResult<Datalogger>> Create(Datalogger datalogger)
        {
            try
            {
                await _context.Dataloggers.InsertOneAsync(datalogger);

                return CreatedAtAction(nameof(Get), new { serialnumber = datalogger.Serialnumber }, datalogger);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while inserting a datalogger: {ex.Message}");
            }
        }

        // PUT api/<DataloggersController>/5
        [HttpPut("{serialnumber}")]
        public async Task<IActionResult> Update(string serialnumber, Datalogger dataloggerInput)
        {
            try
            {
                if (dataloggerInput == null || string.IsNullOrEmpty(serialnumber))
                {
                    return BadRequest("Invalid datalogger or datalogger serialnumber");
                }

                var existingDatalogger = await _context.Dataloggers.Find(d => d.Serialnumber == serialnumber).SingleOrDefaultAsync();

                if (existingDatalogger == null)
                {
                    return NotFound();
                }

                await _context.Dataloggers.ReplaceOneAsync(d => d.Serialnumber == serialnumber, dataloggerInput);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while modifying datalogger {serialnumber}: {ex.Message}");
            }
        }

        // DELETE api/<DataloggersController>/5
        [HttpDelete("{serialnumber}")]
        public async Task<IActionResult> Delete(string serialnumber)
        {
            try
            {
                var datalogger = await _context.Dataloggers.Find(d => d.Serialnumber == serialnumber).SingleOrDefaultAsync();

                if (datalogger == null)
                {
                    return NotFound();
                }

                await _context.Dataloggers.DeleteOneAsync(d => d.Serialnumber == serialnumber);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting datalogger {serialnumber}: {ex.Message}");
            }
        }
    }
}
