using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Participant";

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}