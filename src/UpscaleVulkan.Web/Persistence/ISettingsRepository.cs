namespace UpscaleVulkan.Web.Persistence
{
    using System.Threading.Tasks;

    public interface ISettingsRepository
    {
        Task<TSettings> LoadSettings<TSettings>();
        
        Task SaveSettings<TSettings>(TSettings settings);
    }
}