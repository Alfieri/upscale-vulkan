namespace UpscaleVulkan.Application.Services
{
    using System.Threading.Tasks;

    public interface ISettingsService
    {
        Task EnsurePath(string path);
        
        Task<TSettings> LoadSettingsAsync<TSettings>() where TSettings: new();
        
        Task SaveSettingsAsync<TSettings>(TSettings settings);
    }
}