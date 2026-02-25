using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}
