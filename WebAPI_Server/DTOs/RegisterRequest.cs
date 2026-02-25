namespace WebAPI_Server.DTOs
{
    public class RegisterRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        // optional: nur sinnvoll für Participant                            
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // "Admin" oder "Participant"                                        
        public string Role { get; set; } = "Participant";
    }
}