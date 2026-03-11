using Microsoft.Maui.Devices;
using Models;
using System.Net.Http.Headers;
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

            // optional: falls bereits eingeloggt
            var token = AuthSession.Token?.Trim();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private static string GetBaseApiUrl()
        {
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

        public async Task<List<Participant>> GetParticipantsForEventAsync(int eventId)
        {
            var result = await _client.GetFromJsonAsync<List<Participant>>($"events/{eventId}/participants");
            return result ?? new List<Participant>();
        }
        public async Task<bool> DeleteParticipantAsync(int id)
        {
            var response = await _client.DeleteAsync($"participants/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<Participant?> GetParticipantAsync(int id)
    => await _client.GetFromJsonAsync<Participant>($"participants/{id}");

        public async Task<bool> CreateParticipantAsync(Participant participant)
        {
            var response = await _client.PostAsJsonAsync("participants", participant);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateParticipantAsync(Participant participant)
        {
            var response = await _client.PutAsJsonAsync($"participants/{participant.Id}", participant);
            return response.IsSuccessStatusCode;
        }

        // DEBUG/Diagnose-Variante: gibt Response + Debugtext zurück
        public async Task<(HttpResponseMessage res, string debug)> EnrollInEventDebugAsync(
            int eventId, string firstName, string lastName, string email)
        {
            var token = AuthSession.Token?.Trim();

            if (!string.IsNullOrWhiteSpace(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var debug =
                $"BaseAddress: {_client.BaseAddress}\n" +
                $"AuthHeader: {_client.DefaultRequestHeaders.Authorization}";

            var payload = new
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            var res = await _client.PostAsJsonAsync($"events/{eventId}/enroll", payload);
            return (res, debug);
        }
    }
}