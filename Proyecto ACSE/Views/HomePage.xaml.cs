using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_ACSE.Views
{
    public partial class HomePage : ContentPage
    {
        private LocalDBService _localDBService;
        private bool _showAllCases = false;
        private List<Case> _allCases;
        private bool _isAscendingOrder = false; 
        string _selectedFilter = "Id del Caso";
        bool _isFiltering = false;

        int UserId => UserSessionService.Instance.CurrentUser.Id;
        string UserName => UserSessionService.Instance.CurrentUser.Name;
        bool IsAdmin => UserSessionService.Instance.CurrentUser.Role == User.UserRole.Admin;

        public HomePage()
        {
            InitializeComponent();
            _localDBService = new LocalDBService();
            Shell.SetNavBarIsVisible(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (UserId > 0 && !string.IsNullOrEmpty(UserName))
            {
                UserNameLabel.Text = $"Bienvenido, {UserName}";
                CasesLabel.Text = IsAdmin ? "TODOS LOS CASOS" : "TUS CASOS";
                DynamicNavigationBtn.ImageSource = IsAdmin ? ImageSource.FromFile("user.png") : ImageSource.FromFile("information.png");
                DynamicNavigationBtn.Clicked += IsAdmin ? OnUsersButtonClicked : OnInformationButtonClicked;
                DynamicNavigationLabel.Text = IsAdmin ? "Usuarios" : "Información";
                SearchGrid.IsVisible = IsAdmin;
                await LoadCases();
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

        private async Task LoadCases()
        {
            if (IsAdmin)
                _allCases = await _localDBService.CaseService.GetAllCases();
            else
            {
                _allCases = await _localDBService.CaseService.GetCasesByUserId(UserId);
            }

            var inProgressCases = _allCases.Where(c => c.InProgress).ToList();
            var resolvedCases = _allCases.Where(c => c.Resolved).ToList();

            if (!IsAdmin) _allCases = _allCases.Where(c => !c.InProgress && !c.Resolved).ToList();

            var filteredCases = ApplyFilters(_allCases);

            if (filteredCases.Count > 0)
            {
                if (IsAdmin)
                {
                    CasesCollectionViewAdmin.ItemsSource = filteredCases;
                    CasesCollectionViewAdmin.IsVisible = true;
                    CasesCollectionView.IsVisible = false;
                }
                else
                {
                    CasesCollectionView.ItemsSource = filteredCases;
                    CasesCollectionViewAdmin.IsVisible = false;
                    CasesCollectionView.IsVisible = true;
                }

                NoCasesLabel.IsVisible = false;
            }
            else
            {
                NoCasesLabel.IsVisible = true;
                CasesCollectionView.IsVisible = false;
                CasesCollectionViewAdmin.IsVisible = false;
            }

            InProgressTitle.IsVisible = inProgressCases.Count > 0;
            InProgressCasesCollectionView.ItemsSource = inProgressCases;
            ResolvedTitle.IsVisible = resolvedCases.Count > 0;
            ResolvedCasesCollectionView.ItemsSource = resolvedCases;

            // Mostrar/ocultar elementos de UI basados en filtros
            ShowAllCheckBoxContainer.IsVisible = _allCases.Count > 5 || _isFiltering;
            OrderByButton.IsVisible = _allCases.Count > 1 || _isFiltering;
        }


        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredCases = ApplyFilters(_allCases);

            if (IsAdmin)
            {
                CasesCollectionViewAdmin.ItemsSource = filteredCases;
            }
            else
            {
                CasesCollectionView.ItemsSource = filteredCases;
            }
        }

        private async void OnFilterImageTapped(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Seleccionar filtro", "Cancelar", null, "Id del Caso", "Id del Usuario");

            if (action != null && action != "Cancelar")
            {
                _selectedFilter = action;

                // Actualizar la búsqueda actual si hay texto en el cuadro de búsqueda
                OnSearchBarTextChanged(SearchBar, new TextChangedEventArgs(SearchBar.Text, SearchBar.Text));
            }
        }

        private List<Case> ApplyFilters(List<Case> cases)
        {
            _isFiltering = false;

            // Aplicar búsqueda
            if (!string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                _isFiltering = true;
                var searchText = SearchBar.Text.ToLower();
                if (_selectedFilter == "Id del Caso")
                {
                    cases = cases
                        .Where(c => c.Id.ToString().Contains(searchText))
                        .ToList();
                }
                else
                {
                    cases = cases
                        .Where(c => c.UserId.ToString().Contains(searchText))
                        .ToList();
                }
            }

            // Aplicar ordenación
            cases = _isAscendingOrder
                ? cases.OrderBy(c => c.RegistrationDate).ThenBy(c => c.RegistrationTime).ToList()
                : cases.OrderByDescending(c => c.RegistrationDate).ThenByDescending(c => c.RegistrationTime).ToList();

            // Aplicar límite de 5 casos si es necesario
            if (!_showAllCases)
            {
                cases = cases.Take(5).ToList();
            }

            if (IsAdmin)
            {
                CasesCollectionViewAdmin.IsVisible = cases.Count > 0;
            }
            else
            {
                CasesCollectionView.IsVisible = cases.Count > 0;
            }

            NoCasesLabel.IsVisible = cases.Count <= 0;

            return cases;
        }

        private async void OnShowAllCheckBoxChanged(object sender, CheckedChangedEventArgs e)
        {
            _showAllCases = e.Value;
            await LoadCases();
        }

        private async void OnOrderByButtonClicked(object sender, EventArgs e)
        {
            _isAscendingOrder = !_isAscendingOrder;
            OrderByButton.Text = _isAscendingOrder ? "Ascendente" : "Descendente";
            await LoadCases();
        }

        private async void OnNewCaseClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CasePage));
        }

        private async void OnDeleteCaseClicked(object sender, EventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.BindingContext is Case caseToDelete)
            {
                bool confirm = await DisplayAlert("Confirmar eliminación", "¿Estás seguro de que deseas eliminar este caso?", "Aceptar", "Cancelar");
                if (confirm)
                {
                    await _localDBService.CaseService.DeleteCaseAsync(caseToDelete.Id);
                    await LoadCases();

                    try
                    {
                        var emailService = new EmailService();
                        await emailService.SendCaseDeletionEmailAsync(caseToDelete, UserSessionService.Instance.CurrentUser);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private async void OnCaseTapped(object sender, EventArgs e)
        {
            if (sender is VisualElement element && element.BindingContext is Case selectedCase)
            {
                await Shell.Current.GoToAsync($"{nameof(CasePage)}?caseId={selectedCase.Id}&isViewMode=true");
            }
        }

        private async void OnInformationButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(InformationPage));
        }

        private async void OnUsersButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(UsersPage));
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
}