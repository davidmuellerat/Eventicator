namespace Eventicator.Services
{
    public static class AuthSession
    {
        public static string? Token { get; private set; }
        public static string? Role { get; private set; }
        public static string? Email { get; private set; }
        public static int UserId { get; private set; }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

        public static void Set(string token, string role, string email, int userId)
        {
            Token = token;
            Role = role;
            Email = email;
            UserId = userId;
        }

        public static void Clear()
        {
            Token = null;
            Role = null;
            Email = null;
            UserId = 0;
        }
    }
}