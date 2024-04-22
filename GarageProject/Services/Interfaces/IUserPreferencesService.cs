namespace GarageProject.Services.Interfaces;

public interface IUserPreferencesService
{
    public Task<bool> ExistsThemePreference();
    public Task<string> GetCurrentThemePreference();
    public Task SetCurrentThemePreference(string theme);
}
