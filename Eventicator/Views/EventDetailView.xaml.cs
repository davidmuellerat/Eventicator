using Eventicator.ViewModels;

namespace Eventicator.Views;

public partial class EventDetailView : ContentPage
{
    public EventDetailView(EventDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
