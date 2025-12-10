using Eventicator.ViewModels;

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
}
