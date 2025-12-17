using Models;
using Eventicator.Services;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace Eventicator.ViewModels
{
    public class EventCreateViewModel : BaseViewModel
    {
        private readonly ApiService _api;

        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
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
                await Shell.Current.DisplayAlertAsync("Fehler", "Das Event konnte nicht gespeichert werden.", "OK");
                return;
            }

            WeakReferenceMessenger.Default.Send(new EventsUpdatedMessage());

                await Shell.Current.DisplayAlert("Fehler", "Das Event konnte nicht gespeichert werden.", "OK");
                return;
            }

            MessagingCenter.Send(this, "EventsUpdated");
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
