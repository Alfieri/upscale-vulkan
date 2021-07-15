using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using UpscaleVulkan.Application.Services;
using UpscaleVulkan.Core.Settings;

namespace UpscaleVulkan.Web.Components
{
    public partial class SettingsWaifu2x : ComponentBase
    {
        private Waifu2xSettings settings = new();
        
        [Inject]
        private ISettingsService settingsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.settings = await this.settingsService.LoadSettingsAsync<Waifu2xSettings>();
        }

        private async Task SaveSettings()
        {
            await this.settingsService.SaveSettingsAsync(this.settings);
        }
    }
}