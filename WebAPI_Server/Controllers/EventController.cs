using Microsoft.AspNetCore.Authorization;                                                                                              
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ORM;
using System.Security.Claims;                                                                                                           
using WebAPI_Server.Data;
using WebAPI_Server.DTOs;             

namespace WebAPI_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            return ev;
        }

        // POST: api/events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = ev.Id }, ev);
        }

        // PUT: api/events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event ev)
        {
            if (id != ev.Id)
                return BadRequest();

            _context.Entry(ev).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null)
                return NotFound();

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize(Roles = "Participant")]
        [HttpPost("{eventId}/enroll")]
        public async Task<ActionResult> Enroll(int eventId, EnrollRequest req)
        {
            // Event existiert?                                                                                                                 
            var evExists = await _context.Events.AnyAsync(e => e.Id == eventId);
            if (!evExists) return NotFound("Event not found.");

            // eingeloggter User                                                                                                                
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null) return Unauthorized();

            var userId = int.Parse(userIdStr);

            // User laden                                                                                                                       
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Unauthorized();

            // already enrolled? (wir matchen simpel über Email+EventId)                                                                        
            var email = req.Email.Trim().ToLowerInvariant();
            var already = await _context.Participants.AnyAsync(p => p.EventId == eventId && p.Email.ToLower() == email);
            if (already) return BadRequest("Already enrolled.");

            var participant = new Participant
            {
                EventId = eventId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = email
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
