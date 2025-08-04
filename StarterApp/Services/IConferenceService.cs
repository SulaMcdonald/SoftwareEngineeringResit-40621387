// File: Services/IConferenceService.cs
using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IConferenceService
{
    Task CreateConferenceAsync(Conference conference);
    Task<List<Conference>> GetAllConferencesAsync();
    Task<Conference?> GetConferenceByIdAsync(int id);
    Task UpdateConferenceAsync(Conference conference);
    Task DeleteConferenceAsync(int id);
}
