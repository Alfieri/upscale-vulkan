namespace UpscaleVulkan.Web.Components;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using UpscaleVulkan.Application.Services;
using UpscaleVulkan.Core.Settings;

public partial class SettingsWaifu2xComponent : ComponentBase
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