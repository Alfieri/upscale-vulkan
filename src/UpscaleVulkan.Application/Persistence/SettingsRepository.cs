namespace UpscaleVulkan.Application.Persistence
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class SettingsRepository : ISettingsRepository
    {
        private readonly string appData;

        public SettingsRepository()
        {
            this.appData =
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,
                        Environment.SpecialFolderOption.DoNotVerify), "upscale.vulkan");
            Directory.CreateDirectory(this.appData);
        }

        public async Task<TSettings?> LoadSettingsAsync<TSettings>()
        {
            Type type = typeof(TSettings);
            string filename = this.CreateFileName(type);
            if (!File.Exists(filename))
            {
                return default;
            }
            
            await using FileStream file = File.OpenRead(filename);
            return await JsonSerializer.DeserializeAsync<TSettings>(file);
        }

        public async Task SaveSettingsAsync<TSettings>(TSettings settings)
        {
            await using FileStream file = File.Create(this.CreateFileName(settings!.GetType()));
            
            await JsonSerializer.SerializeAsync(file, settings);
        }

        private string CreateFileName(MemberInfo settingsType)
        {
            return $"{Path.Combine(this.appData, settingsType.Name)}.json";
        }
    }
}