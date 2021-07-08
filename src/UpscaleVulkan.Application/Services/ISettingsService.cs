namespace UpscaleVulkan.Application.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISettingsService
    {
        Task<TSettings> LoadSettingsAsync<TSettings>() where TSettings: new();
        Task SaveSettingsAsync<TSettings>(TSettings settings);
    }
}