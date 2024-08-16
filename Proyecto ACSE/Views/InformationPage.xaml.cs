namespace Proyecto_ACSE.Views
{
    public partial class InformationPage : ContentPage
    {
        private LocalDBService _localDBService;
        string UserName => UserSessionService.Instance.CurrentUser.Name;

        public InformationPage()
        {
            InitializeComponent();
            _localDBService = new LocalDBService();
            Shell.SetNavBarIsVisible(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UserNameLabel.Text = $"Bienvenido, {UserName}";
        }

        protected override bool OnBackButtonPressed()
        {
            ShowExitAlert();
            return true;
        }

        private async void ShowExitAlert()
        {
            bool answer = await DisplayAlert("Confirmar", "�Est�s seguro de que deseas cerrar sesi�n?", "Aceptar", "Cancelar");
            if (answer)
            {
                UserSessionService.Instance.ClearSession();

                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        private async void OnCasesButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(HomePage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Navegaci�n fallida: {ex.Message}", "Aceptar");
            }
        }

        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(SettingsPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Navegaci�n fallida: {ex.Message}", "Aceptar");
            }
        }
    }
}