using Eventicator.ViewModels;

namespace Eventicator.Views;

public partial class EventDetailView : ContentPage
{
    public EventDetailView(EventDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is EventDetailViewModel vm)
        {
            await vm.RefreshFromServerAsync();
        }
    }
}
