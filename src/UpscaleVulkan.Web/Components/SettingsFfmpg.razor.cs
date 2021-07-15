namespace UpscaleVulkan.Web.Components
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Application.Services;
    using Core.Settings;
    
    public partial class SettingsFfmpg : ComponentBase
    {
        private FfmpegSettings settings = new();
        
        [Inject]
        private ISettingsService settingsService { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            this.settings = await this.settingsService.LoadSettingsAsync<FfmpegSettings>();
        }

        private async Task SaveSettings()
        {
            await this.settingsService.SaveSettingsAsync(this.settings);
        }
    }
}