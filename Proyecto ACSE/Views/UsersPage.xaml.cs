namespace Proyecto_ACSE.Views;

public partial class UsersPage : ContentPage
{
    private LocalDBService _localDBService;
    string UserName => UserSessionService.Instance.CurrentUser.Name;
    private List<User> _allUsers;

    public UsersPage()
	{
		InitializeComponent();
        _localDBService = new LocalDBService();
        Shell.SetNavBarIsVisible(this, false);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        UserNameLabel.Text = $"Bienvenido, {UserName}";
        await LoadUsers();
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

    private async Task LoadUsers()
    {
        _allUsers = await _localDBService.UserService.GetAllUsers();

        if (_allUsers.Count > 0)
        {
            UsersCollectionView.ItemsSource = _allUsers;
            UsersCollectionView.IsVisible = true;
            NoUsersLabel.IsVisible = false;
        }
        else
        {
            UsersCollectionView.IsVisible = false;
            NoUsersLabel.IsVisible = true;
        }
    }

    private async void OnNewUserClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async void OnUserTapped(object sender, EventArgs e)
    {
        if (sender is VisualElement element && element.BindingContext is User selectedUser)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}?userId={selectedUser.Id}&isEditMode=true");
        }
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            // Si el cuadro de búsqueda está vacío, mostrar todos los casos
            UsersCollectionView.ItemsSource = _allUsers;
        }
        else
        {
            // Filtrar la lista de casos por ID de caso o ID de usuario
            var searchText = e.NewTextValue.ToLower();
            var filteredCases = _allUsers
                .Where(u => u.Id.ToString().Contains(searchText) || u.Name.ToLower().Contains(searchText))
                .ToList();
            UsersCollectionView.ItemsSource = filteredCases;
        }
    }

    private async void OnDeleteUserClicked(object sender, EventArgs e)
    {
        if (sender is Button deleteButton && deleteButton.BindingContext is User userToDelete)
        {
            if (userToDelete.Name == "AnonymousReports" || userToDelete.Email == "admin")
            {
                await DisplayAlert("Error", "Este usuario no se puede elminar", "Aceptar");
            }
            else
            {

                bool confirm = await DisplayAlert("Confirmar eliminación", "¿Estás seguro de que deseas eliminar este usuario?", "Aceptar", "Cancelar");
                if (confirm)
                {
                    await _localDBService.UserService.DeleteUserAsync(userToDelete.Id);
                    await LoadUsers();
                }
            }
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

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navegación fallida: {ex.Message}", "Aceptar");
        }
    }
}