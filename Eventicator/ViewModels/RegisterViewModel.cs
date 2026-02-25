using System.Windows.Input;
using Eventicator.Services;

namespace Eventicator.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly AuthApiService _auth = new AuthApiService();

        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public string Role { get; set; } = "Participant";

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await Register());
        }

        private async Task Register()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var ok = await _auth.RegisterAsync(Email, Password, Role, FirstName, LastName);
                if (!ok)
                {
                    await Shell.Current.DisplayAlert("Fehl er", "Registrierung fehlgeschlagen.", "OK");
                    return;
                }

                await Shell.Current.DisplayAlert("OK", "Registriert. Du kannst dich jetzt einloggen.", "OK");
                await Shell.Current.Navigation.PopAsync(); // zurück zu Login                                                          
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}