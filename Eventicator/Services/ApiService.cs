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
                BaseAddress = new Uri("http://localhost:5226/api/")  // API URL
            };
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
