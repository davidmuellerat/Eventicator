using Eventicator.Services;
using Eventicator.Views;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;
=======
using Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Eventicator.ViewModels
{
    public class EventListViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Event> Events { get; set; }
            = new ObservableCollection<Event>();

        public ICommand LoadEventsCommand { get; }
        public ICommand AddEventCommand { get; }
        public EventListViewModel()
        {
            _apiService = new ApiService();
            LoadEventsCommand = new Command(async () => await LoadEvents());
            AddEventCommand = new Command(async () =>
               await Shell.Current.Navigation.PushAsync(new EventCreateView()));

            WeakReferenceMessenger.Default.Register<EventListViewModel, EventsUpdatedMessage>(this, async (recipient, message) =>
            {
                await recipient.LoadEvents();
            });

            MessagingCenter.Subscribe<EventCreateViewModel>(this, "EventsUpdated", async _ => await LoadEvents());
            MessagingCenter.Subscribe<EventDetailViewModel>(this, "EventsUpdated", async _ => await LoadEvents());
        }


        public async Task LoadEvents()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Events.Clear();
                var items = await _apiService.GetEventsAsync();

                foreach (var ev in items)
                    Events.Add(ev);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
