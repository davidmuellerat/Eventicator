namespace WebAPI_Server.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = "";
        public string Role { get; set; } = "";
        public string Email { get; set; } = "";
        public int UserId { get; set; }
    }
}