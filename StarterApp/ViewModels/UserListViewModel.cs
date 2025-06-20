using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public class UserListViewModel : INotifyPropertyChanged
{
    private readonly AppDbContext _context;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authenticationService;

    private ObservableCollection<UserListItem> _users = new();
    private ObservableCollection<UserListItem> _filteredUsers = new();
    private string _selectedRoleFilter = "All";
    private string _searchText = string.Empty;
    private bool _isLoading = false;
    private bool _isRefreshing = false;

    public UserListViewModel(AppDbContext context, INavigationService navigationService, IAuthenticationService authenticationService)
    {
        _context = context;
        _navigationService = navigationService;
        _authenticationService = authenticationService;

        LoadUsersCommand = new Command(async () => await LoadUsersAsync());
        RefreshCommand = new Command(async () => await RefreshUsersAsync());
        UserSelectedCommand = new Command<UserListItem>(async (user) => await NavigateToUserDetailAsync(user));
        CreateUserCommand = new Command(async () => await NavigateToCreateUserAsync());
        
        RoleFilterOptions = new ObservableCollection<string> { "All" };
        foreach (var role in RoleConstants.AllRoles)
        {
            RoleFilterOptions.Add(role);
        }

        // Load users when view model is created
        _ = Task.Run(LoadUsersAsync);
    }

    public ObservableCollection<UserListItem> Users
    {
        get => _users;
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<UserListItem> FilteredUsers
    {
        get => _filteredUsers;
        set
        {
            _filteredUsers = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> RoleFilterOptions { get; }

    public string SelectedRoleFilter
    {
        get => _selectedRoleFilter;
        set
        {
            _selectedRoleFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged();
        }
    }

    public bool IsAdmin => _authenticationService.HasRole(RoleConstants.Admin);

    public ICommand LoadUsersCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand UserSelectedCommand { get; }
    public ICommand CreateUserCommand { get; }

    private async Task LoadUsersAsync()
    {
        if (!IsAdmin)
        {
            await _navigationService.NavigateToAsync("//MainPage");
            return;
        }

        IsLoading = true;
        try
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.IsActive)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();

            var userItems = users.Select(u => new UserListItem
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                FullName = u.FullName,
                CreatedAt = u.CreatedAt ?? DateTime.MinValue,
                IsActive = u.IsActive,
                Roles = u.UserRoles
                    .Where(ur => ur.IsActive)
                    .Select(ur => ur.Role.Name)
                    .ToList(),
                RolesDisplay = string.Join(", ", u.UserRoles
                    .Where(ur => ur.IsActive)
                    .Select(ur => ur.Role.Name))
            }).ToList();

            Users = new ObservableCollection<UserListItem>(userItems);
            ApplyFilters();
        }
        catch (Exception ex)
        {
            // Handle error - could show alert or log
            System.Diagnostics.Debug.WriteLine($"Error loading users: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task RefreshUsersAsync()
    {
        IsRefreshing = true;
        await LoadUsersAsync();
        IsRefreshing = false;
    }

    private void ApplyFilters()
    {
        var filtered = Users.AsEnumerable();

        // Apply role filter
        if (SelectedRoleFilter != "All")
        {
            filtered = filtered.Where(u => u.Roles.Contains(SelectedRoleFilter));
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchLower = SearchText.ToLower();
            filtered = filtered.Where(u => 
                u.FullName.ToLower().Contains(searchLower) ||
                u.Email.ToLower().Contains(searchLower) ||
                u.RolesDisplay.ToLower().Contains(searchLower));
        }

        FilteredUsers = new ObservableCollection<UserListItem>(filtered);
    }

    private async Task NavigateToUserDetailAsync(UserListItem user)
    {
        if (user != null)
        {
            await _navigationService.NavigateToAsync($"UserDetailPage?userId={user.Id}");
        }
    }

    private async Task NavigateToCreateUserAsync()
    {
        await _navigationService.NavigateToAsync("UserDetailPage?userId=0");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class UserListItem
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
    public string RolesDisplay { get; set; } = string.Empty;
}