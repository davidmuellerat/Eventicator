using Eventicator.ViewModels;

namespace Eventicator.Views;

public partial class ParticipantListView : ContentPage
{
    public ParticipantListView()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ParticipantListViewModel vm)
            await vm.LoadAsync();
    }
}