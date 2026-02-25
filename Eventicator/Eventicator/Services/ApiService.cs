using Microsoft.Maui.Devices;
using Models;
using System.Net.Http.Json;

namespace Eventicator.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(GetBaseApiUrl())
            };
        }

        private static string GetBaseApiUrl()
        {
            // When the WebAPI server runs alongside the emulator, localhost must be redirected
            // to the host machine. On Android emulators, 10.0.2.2 resolves to the host.
            return DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5226/api/"
                : "http://localhost:5226/api/";
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            var result = await _client.GetFromJsonAsync<List<Event>>("events");
            return result ?? new List<Event>();
        }
        public async Task<Event?> GetEventAsync(int id)
        {
            return await _client.GetFromJsonAsync<Event>($"events/{id}");
        }

        public async Task<bool> CreateEventAsync(Event ev)
        {
            var response = await _client.PostAsJsonAsync("events", ev);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEventAsync(Event ev)
        {
            var response = await _client.PutAsJsonAsync($"events/{ev.Id}", ev);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var response = await _client.DeleteAsync($"events/{id}");
            return response.IsSuccessStatusCode;
        }

    }
}
