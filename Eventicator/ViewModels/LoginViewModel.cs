using System.Windows.Input;
using Eventicator.Services;

namespace Eventicator.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthApiService _auth = new AuthApiService();

        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public ICommand LoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
            OpenRegisterCommand = new Command(async () => await Shell.Current.Navigation.PushAsync(new Views.RegisterView()));
        }

        private async Task Login()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var ok = await _auth.LoginAsync(Email, Password);
                if (!ok)
                {
                    await Shell.Current.DisplayAlert("Fehler", "Login fehlgeschlagen.", "OK");
                    return;
                }

                // Nach Login zur Event-Liste (Shell Route siehe AppShell unten)                                                        
                await Shell.Current.GoToAsync("//events");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}