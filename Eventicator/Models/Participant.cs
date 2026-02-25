using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Participant
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;

        public string? TicketCode { get; set; }

        public Event? Event { get; set; }
    }
}
