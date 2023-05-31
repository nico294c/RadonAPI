using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RadonAPI.Data;
using RadonAPI.Models;

namespace RadonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly MongoDBContext _context;

        public CustomersController(MongoDBContext context)
        {
            _context = context;
        }

        // GET: api/<CustomersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customers = await _context.Customers.Find(_ => true).ToListAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving customers: {ex.Message}");
            }
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            try
            {
                var customer = await _context.Customers.Find(c => c.Id == id).SingleOrDefaultAsync();

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving customer {id}: {ex.Message}");
            }
        }

        // POST api/<CustomersController>
        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer customer)
        {
            try
            {
                await _context.Customers.InsertOneAsync(customer);

                return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while inserting a customer: {ex.Message}");
            }
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Customer customerInput)
        {
            try
            {
                if (customerInput == null || string.IsNullOrEmpty(id))
                {
                    return BadRequest("Invalid customer or customer ID");
                }

                var existingCustomer = await _context.Customers.Find(c => c.Id == id).SingleOrDefaultAsync();

                if (existingCustomer == null)
                {
                    return NotFound();
                }

                await _context.Customers.ReplaceOneAsync(c => c.Id == id, customerInput);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while modifying customer {id}: {ex.Message}");
            }
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var customer = await _context.Customers.Find(c => c.Id == id).SingleOrDefaultAsync();

                if (customer == null)
                {
                    return NotFound();
                }

                await _context.Customers.DeleteOneAsync(c => c.Id == id);

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting customer {id}: {ex.Message}");
            }
        }
    }
}
