using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ORM;
using WebAPI_Server.Data;

namespace WebAPI_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParticipantsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/participants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Participant>> GetParticipant(int id)
        {
            var p = await _context.Participants.FindAsync(id);
            if (p == null)
                return NotFound();

            return p;
        }

        // GET: api/participants/by-event/3  (alle Teilnehmer eines Events)
        [HttpGet("by-event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Participant>>> GetByEvent(int eventId)
        {
            return await _context.Participants
                .Where(p => p.EventId == eventId)
                .ToListAsync();
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<Participant>> CreateParticipant(Participant p)
        {
            _context.Participants.Add(p);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParticipant), new { id = p.Id }, p);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParticipant(int id, Participant p)
        {
            if (id != p.Id)
                return BadRequest();

            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            var p = await _context.Participants.FindAsync(id);
            if (p == null)
                return NotFound();

            _context.Participants.Remove(p);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
