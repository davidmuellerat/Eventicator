using Microsoft.Maui.Devices;
using System.Net.Http.Json;

namespace Eventicator.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _client;

        public AuthApiService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(GetBaseApiUrl())
            };
        }

        private static string GetBaseApiUrl()
        {
            return DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5226/api/"
                : "http://localhost:5226/api/";
        }

        public async Task<bool> RegisterAsync(string email, string password, string role, string? firstName, string? lastName)
        {
            var payload = new
            {
                Email = email,
                Password = password,
                Role = role,
                FirstName = firstName,
                LastName = lastName
            };

            var res = await _client.PostAsJsonAsync("auth/register", payload);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var payload = new { Email = email, Password = password };

            var res = await _client.PostAsJsonAsync("auth/login", payload);
            if (!res.IsSuccessStatusCode) return false;

            var data = await res.Content.ReadFromJsonAsync<AuthResponse>();
            if (data == null) return false;

            AuthSession.Set(data.Token.Trim(), data.Role, data.Email, data.UserId);
            return true;
        }

        private class AuthResponse
        {
            public string Token { get; set; } = "";
            public string Role { get; set; } = "";
            public string Email { get; set; } = "";
            public int UserId { get; set; }
        }
    }
}