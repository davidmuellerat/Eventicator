using Models;
using Eventicator.Services;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace Eventicator.ViewModels
{
    public class EventDetailViewModel : BaseViewModel
    {
        private readonly ApiService _api;
        private readonly INavigation _navigation;

        private Event _event;
        public Event Event
        {
            get => _event;
            set
            {
                if (_event == value) return;
                _event = value;
                OnPropertyChanged(nameof(Event));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public EventDetailViewModel(Event ev, INavigation nav)
        {
            _api = new ApiService();
            _navigation = nav;
            Event = ev;

            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(async () => await Delete());
        }

        public async Task RefreshFromServerAsync()
        {
            var updated = await _api.GetEventAsync(Event.Id);
            if (updated != null)
            {
                Event = updated;
            }
        }

        private async Task Save()
        {
            var success = await _api.UpdateEventAsync(Event);

            if (!success)
            {
                await Shell.Current.DisplayAlert("Fehler", "Das Event konnte nicht aktualisiert werden.", "OK");
                return;
            }

            await RefreshFromServerAsync();
            MessagingCenter.Send(this, "EventsUpdated");
            await _navigation.PopAsync();
        }

        private async Task Delete()
        {
            var success = await _api.DeleteEventAsync(Event.Id);

            if (!success)
            {
                await Shell.Current.DisplayAlert("Fehler", "Das Event konnte nicht gelöscht werden.", "OK");
                return;
            }

            MessagingCenter.Send(this, "EventsUpdated");
            await _navigation.PopAsync();
        }
    }
}
