namespace Proyecto_ACSE
{
    public partial class App : Application
    {
        private LocalDBService _localDBService;

        public App()
        {
            InitializeComponent();
            _localDBService = new LocalDBService();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await _localDBService.UserService.EnsureAdminUserExists();
            await _localDBService.UserService.EnsureAnonymousUserExists();
        }
    }
}
