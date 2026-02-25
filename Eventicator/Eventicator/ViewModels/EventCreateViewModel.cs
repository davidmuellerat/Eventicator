using Models;
using Eventicator.Services;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace Eventicator.ViewModels
{
    public class EventCreateViewModel : BaseViewModel
    {
        private readonly ApiService _api;

        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public ICommand CreateCommand { get; }

        public EventCreateViewModel()
        {
            _api = new ApiService();
            CreateCommand = new Command(async () => await Create());
        }

        private async Task Create()
        {
            var newEvent = new Event
            {
                Title = Title,
                Location = Location,
                Description = Description,
                Date = Date
            };

            var success = await _api.CreateEventAsync(newEvent);

            if (!success)
            {
                await Shell.Current.DisplayAlert("Fehler", "Das Event konnte nicht gespeichert werden.", "OK");
                return;
            }

            await Shell.Current.Navigation.PopAsync();
        }
    }
}
