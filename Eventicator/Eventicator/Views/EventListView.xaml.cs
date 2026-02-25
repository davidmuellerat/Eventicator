using Eventicator.ViewModels;
using Models;
using System.Linq;

namespace Eventicator.Views;

public partial class EventListView : ContentPage
{
    public EventListView()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as EventListViewModel;
        vm?.LoadEventsCommand.Execute(null);
    }

    private async void OnEventSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Event selectedEvent)
        {
            return;
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }

        var detailVm = new EventDetailViewModel(selectedEvent, Navigation);
        await Navigation.PushAsync(new EventDetailView(detailVm));
    }
}
