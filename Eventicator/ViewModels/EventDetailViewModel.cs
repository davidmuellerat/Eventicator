using Eventicator.Services;
using Microsoft.Maui.Controls;
using Models;
using System.Collections.Generic;
using System.Windows.Input;
using System.Xml.Linq;

namespace Eventicator.ViewModels
{

    public class EventDetailViewModel : BaseViewModel
    {
        private readonly ApiService _api;
        private readonly INavigation _navigation;
        public ICommand EnrollCommand { get; }

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
        public ICommand OpenParticipantsCommand { get; }   

        public EventDetailViewModel(Event ev, INavigation nav)
        {
            _api = new ApiService();
            _navigation = nav;
            Event = ev;
            EnrollCommand = new Command(async () => await Enroll());

            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(async () => await Delete());

            OpenParticipantsCommand = new Command(async () => await OpenParticipants());
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

            await _navigation.PopAsync();
        }

        private async Task OpenParticipants()
        {
            var page = new Views.ParticipantListView();
            page.BindingContext = new ParticipantListViewModel(Event.Id, _navigation);
            await _navigation.PushAsync(page);
        }
        private async Task Enroll()
        {
            if (!AuthSession.IsLoggedIn)
            {
                await Shell.Current.DisplayAlert("Login nötig", "Bitte zuerst einloggen.", "OK");
                await Shell.Current.GoToAsync("//login");
                return;
            }

            var firstName = await Shell.Current.DisplayPromptAsync("Einschreiben", "Vorname:");
            if (string.IsNullOrWhiteSpace(firstName)) return;

            var lastName = await Shell.Current.DisplayPromptAsync("Einschreiben", "Nachname:");
            if (string.IsNullOrWhiteSpace(lastName)) return;

            var defaultEmail = AuthSession.Email ?? "";
            var email = await Shell.Current.DisplayPromptAsync("Einschreiben", "Email:", initialValue: defaultEmail);
            if (string.IsNullOrWhiteSpace(email)) return;

            var ok = await _api.EnrollInEventAsync(Event.Id, firstName, lastName, email);

            if (!ok)
            {
                await Shell.Current.DisplayAlert("Fehler", "Einschreiben fehlgeschlagen.", "OK");
                return;
            }

            await Shell.Current.DisplayAlert("OK", "Du bist eingeschrieben.", "OK");
            await RefreshFromServerAsync();
        }
    }
}