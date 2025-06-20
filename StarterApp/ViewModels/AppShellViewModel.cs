// ViewModels/AppShellViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace StarterApp.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<MenuBarItem> DynamicMenuBarItems { get; } = new();

        public AppShellViewModel()
        {
            Title = "StarterApp";
        }

        public AppShellViewModel(IAuthenticationService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
            _authService.AuthenticationStateChanged += OnAuthenticationStateChanged;
            Title = "StarterApp";

            // Initial setup of menu items based on current role
            // UpdateMenuItemsForRole();
        }

        // Commands for your menu items
        private bool CanExecuteGuestAction() => _authService.HasRole("Guest");
        private bool CanExecuteUserAction() => _authService.HasRole("OrdinaryUser");
        private bool CanExecuteAdminAction()
        {
            return _authService.HasRole("Admin");
        }
        private bool CanExecuteAuthenticatedAction()
        {
            return _authService.IsAuthenticated;
        }
        private void OnAuthenticationStateChanged(object? sender, bool isAuthenticated)
        {
            LogoutCommand.NotifyCanExecuteChanged();
            NavigateToProfileCommand.NotifyCanExecuteChanged();
            NavigateToSettingsCommand.NotifyCanExecuteChanged();
            NavigateToAdminCommand.NotifyCanExecuteChanged();
            Debug.WriteLine($"Authentication state changed: {isAuthenticated}");
            Debug.WriteLine($"Current user is admin: {_authService.HasRole("Admin")}");
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAdminAction))]
        private async Task NavigateToAdminAsync()
        {
            await _navigationService.NavigateToAsync("TempPage");
        }
        [RelayCommand(CanExecute = nameof(CanExecuteGuestAction))]
        private async Task GuestActionAsync()
        {
            await _navigationService.NavigateToAsync("PublicInfoPage");
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAuthenticatedAction))]
        private async Task NavigateToProfileAsync()
        {
            await _navigationService.NavigateToAsync("TempPage");
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAuthenticatedAction))]
        private async Task NavigateToSettingsAsync()
        {
            await _navigationService.NavigateToAsync("TempPage");
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAuthenticatedAction))]
        private async Task LogoutAsync()
        {
            await _authService.LogoutAsync();
            await _navigationService.NavigateToAsync("LoginPage");

            LogoutCommand.NotifyCanExecuteChanged();
            NavigateToProfileCommand.NotifyCanExecuteChanged();
            NavigateToSettingsCommand.NotifyCanExecuteChanged();
            NavigateToAdminCommand.NotifyCanExecuteChanged();
            // ... and any other commands tied to roles
        }
        // If you're adding/removing entire MenuBarItems dynamically, you'd do it here:
        // UpdateDynamicMenuBarItems(); // See alternative below
        

        // Example for dynamically adding/removing MenuBarItems (more complex)
        // private void UpdateDynamicMenuBarItems()
        // {
        //     DynamicMenuBarItems.Clear();
        //     if (_authService.HasRole("Admin"))
        //     {
        //         // Add admin-specific menu items to DynamicMenuBarItems
        //         // This would be done in code, creating MenuBarItem and MenuItem objects
        //         var adminMenu = new MenuBarItem { Text = "Admin Tools" };
        //         adminMenu.Add(new MenuItem { Text = "Manage Users", Command = AdminActionCommand });
        //         DynamicMenuBarItems.Add(adminMenu);
        //     }
        // }
    }
}