namespace UpscaleVulkan.Application.Services
{
    using System.IO;
    using System.Threading.Tasks;
    using Persistence;

    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public async Task EnsurePath(string path)
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            });
        }

        public async Task<TSettings> LoadSettingsAsync<TSettings>() where TSettings: new()
        {
            var settings = await this.settingsRepository.LoadSettingsAsync<TSettings>();
            return settings ?? new TSettings();
        }

        public async Task SaveSettingsAsync<TSettings>(TSettings settings)
        {
            await this.settingsRepository.SaveSettingsAsync(settings);
        }
    }
}