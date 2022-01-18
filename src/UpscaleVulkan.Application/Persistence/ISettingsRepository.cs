namespace UpscaleVulkan.Application.Persistence
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISettingsRepository
    {
        Task<TSettings?> LoadSettingsAsync<TSettings>();
        
        Task SaveSettingsAsync<TSettings>(TSettings settings);
    }
}