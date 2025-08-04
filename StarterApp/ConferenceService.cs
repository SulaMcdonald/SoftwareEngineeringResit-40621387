using StarterApp.Database.Data;
using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace StarterApp.Services;

public class ConferenceService : IConferenceService
{
    private readonly AppDbContext _dbContext;

    public ConferenceService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateConferenceAsync(Conference conference)
    {
        _dbContext.Conferences.Add(conference);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Conference>> GetAllConferencesAsync()
    {
        return await _dbContext.Conferences.ToListAsync();
    }

    public async Task<Conference?> GetConferenceByIdAsync(int id)
    {
        return await _dbContext.Conferences.FindAsync(id);
    }

    public async Task UpdateConferenceAsync(Conference conference)
    {
        _dbContext.Conferences.Update(conference);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteConferenceAsync(int id)
    {
        var conference = await _dbContext.Conferences.FindAsync(id);
        if (conference != null)
        {
            _dbContext.Conferences.Remove(conference);
            await _dbContext.SaveChangesAsync();
        }
    }
}
