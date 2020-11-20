using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleEmailsSendler.Models;
using MultipleEmailsSendler.Service;

namespace MultipleEmailsSendler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        private readonly AppDataContext _context;

        public Test(AppDataContext context)
        {
            _context = context;
        }

        // GET: api/Test
        [HttpGet]
        public IEnumerable<Emails> GetEmail()
        {
            return _context.Emails;
        }

        // GET: api/Test/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = await _context.Emails.FindAsync(id);

            if (email == null)
            {
                return NotFound();
            }

            return Ok(email);
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmail([FromRoute] int id, [FromBody] Emails email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != email.Id)
            {
                return BadRequest();
            }

            _context.Entry(email).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Test
        [HttpPost]
        public async Task<IActionResult> PostEmail([FromBody] Emails email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Emails.Add(email);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmail", new { id = email.Id }, email);
        }

        // DELETE: api/Test/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmail([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = await _context.Emails.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }

            _context.Emails.Remove(email);
            await _context.SaveChangesAsync();

            return Ok(email);
        }

        private bool EmailExists(int id)
        {
            return _context.Emails.Any(e => e.Id == id);
        }
    }
}