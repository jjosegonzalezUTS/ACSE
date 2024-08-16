using Proyecto_ACSE.Views;

namespace Proyecto_ACSE
{
    public partial class MainPage : ContentPage
    {
        private LocalDBService _localDBService;

        public MainPage()
        {
            InitializeComponent();
            _localDBService = new LocalDBService();
            Shell.SetNavBarIsVisible(this, false);
        }

        protected override void OnAppearing()
        {
            User.Text = string.Empty;
            Password.Text = string.Empty;
        }

        private async void OnLogIn(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(User.Text) || string.IsNullOrWhiteSpace(Password.Text))
            {
                await DisplayAlert("Error", "Todos los campos son obligatorios.", "Aceptar");
            }
            else
            {
                var user = await _localDBService.UserService.GetUserByEmailAndPassword(User.Text, Password.Text);
                if (user != null)
                {
                    // Guardar los detalles del usuario en el servicio de sesión
                    UserSessionService.Instance.CurrentUser = user;
                    HideKeyboard();
                    await Shell.Current.GoToAsync(nameof(HomePage));
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "Aceptar");
                }
            }
        }

        private async void OnRegister(object sender, EventArgs e)
        {
            HideKeyboard();
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        private void HideKeyboard()
        {
            User.IsEnabled = false;
            Password.IsEnabled = false;
            User.IsEnabled = true;
            Password.IsEnabled = true;
        }
    }
}
