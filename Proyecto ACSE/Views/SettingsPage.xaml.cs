namespace Proyecto_ACSE.Views;

public partial class SettingsPage : ContentPage
{
    private LocalDBService _localDBService;
    string UserName => UserSessionService.Instance.CurrentUser.Name;
    int UserId => UserSessionService.Instance.CurrentUser.Id;
    bool IsAdmin => UserSessionService.Instance.CurrentUser.Role == User.UserRole.Admin;

    public SettingsPage()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        _localDBService = new LocalDBService();
        UserNameLabel.Text = UserName;
        LoadCaseCount();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DynamicNavigationBtn.ImageSource = IsAdmin ? ImageSource.FromFile("user.png") : ImageSource.FromFile("information.png");
        DynamicNavigationBtn.Clicked += IsAdmin ? OnUsersButtonClicked : OnInformationButtonClicked;
        DynamicNavigationLabel.Text = IsAdmin ? "Usuarios" : "Información";
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmar", "¿Estás seguro de que deseas cerrar sesión?", "Aceptar", "Cancelar");
        if (confirm)
        {
            UserSessionService.Instance.ClearSession();
            await Shell.Current.GoToAsync("//MainPage");
        }
    }

    protected override bool OnBackButtonPressed()
    {
        ShowExitAlert();
        return true;
    }

    private async void ShowExitAlert()
    {
        bool answer = await DisplayAlert("Confirmar", "¿Estás seguro de que deseas cerrar sesión?", "Aceptar", "Cancelar");
        if (answer)
        {
            UserSessionService.Instance.ClearSession();

            await Shell.Current.GoToAsync("//MainPage");
        }
    }

    private async void OnInformationButtonClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(InformationPage));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navegación fallida: {ex.Message}", "Aceptar");
        }
    }

    private async void OnUsersButtonClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(UsersPage));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navegación fallida: {ex.Message}", "Aceptar");
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
            await DisplayAlert("Error", $"Navegación fallida: {ex.Message}", "Aceptar");
        }
    }

    private async void LoadCaseCount()
    {
        List<Case> userCases = await _localDBService.CaseService.GetCasesByUserId(UserId);
        int caseCount = userCases.Count;
        CaseCountLabel.Text = caseCount <= 0 ? "El usuario no cuenta con casos registrados." : $"Número de casos registrados por el usuario: {caseCount}";
    }

}