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
        public async Task<IEnumerable<Log>> Get()
        {
            return (IEnumerable<Log>)await _context.Logs.Find(_ => true).ToListAsync();
        }

        // GET api/<LogsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> Get(string id)
        {
            var log = await _context.Logs.Find(l => l.Id == id).FirstOrDefaultAsync();

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }

        // POST api/<LogsController>
        [HttpPost]
        public async Task<ActionResult<Log>> Create(Log log)
        {
            await _context.Logs.InsertOneAsync(log);
            return CreatedAtRoute(new { id = log.Id }, log);
        }

        // PUT api/<LogsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Log logInput)
        {
            var log = await _context.Logs.Find(l => l.Id == id).FirstOrDefaultAsync();

            if (log == null)
            {
                return NotFound();
            }

            await _context.Logs.ReplaceOneAsync(l => l.Id == id, logInput);

            return NoContent();
        }

        // DELETE api/<LogsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var log = await _context.Logs.Find(l => l.Id == id).FirstOrDefaultAsync();

            if (log == null)
            {
                return NotFound();
            }

            await _context.Logs.DeleteOneAsync(l => l.Id == id);

            return NoContent();
        }
    }
}
