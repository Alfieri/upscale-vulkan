namespace UpscaleVulkan.Web.Services
{
    using System.Reflection.Emit;
    using System.Threading.Tasks;

    public interface ISettingsService
    {
        Task<TSettings> LoadSettings<TSettings>() where TSettings: new();
    }

    public class SettingsService : ISettingsService
    {
        public Task<TSettings> LoadSettings<TSettings>() where TSettings: new()
        {
            return Task.FromResult(new TSettings());
        }
    }
}