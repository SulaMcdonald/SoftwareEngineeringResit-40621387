using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class CreateConferenceViewModel : BaseViewModel
{
    private readonly IConferenceService _conferenceService;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string location = string.Empty;
    [ObservableProperty] private DateTime date = DateTime.Today;
    [ObservableProperty] private TimeSpan time = TimeSpan.FromHours(9);
    [ObservableProperty] private string capacity = string.Empty;

    public CreateConferenceViewModel(IConferenceService conferenceService, INavigationService navigationService)
    {
        _conferenceService = conferenceService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task CreateConferenceAsync()
    {
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Location) || string.IsNullOrWhiteSpace(Capacity))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "All fields are required.", "OK");
            return;
        }

        if (!int.TryParse(Capacity, out int parsedCapacity))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Capacity must be a number.", "OK");
            return;
        }

        var conference = new Conference
        {
            Title = Title,
            Location = Location,
            Date = Date,
            Time = DateTime.Today.Add(Time),
            Capacity = parsedCapacity,
            SpeakerId = 1 // TEMP
        };

        await _conferenceService.CreateConferenceAsync(conference);
        await Application.Current.MainPage.DisplayAlert("Success", "Conference created!", "OK");

        await _navigationService.NavigateToAsync("MainPage");
    }
}
