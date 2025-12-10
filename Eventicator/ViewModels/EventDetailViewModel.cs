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

        public Event Event { get; set; }

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

        private async Task Save()
        {
            await _api.UpdateEventAsync(Event);
            MessagingCenter.Send(this, "EventsUpdated");
            await _navigation.PopAsync();
        }

        private async Task Delete()
        {
            await _api.DeleteEventAsync(Event.Id);
            MessagingCenter.Send(this, "EventsUpdated");
            await _navigation.PopAsync();
        }
    }
}
