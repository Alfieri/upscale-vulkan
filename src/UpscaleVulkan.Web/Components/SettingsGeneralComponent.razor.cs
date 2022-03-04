namespace UpscaleVulkan.Web.Components;

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Application.Services;
using Core.Settings;
    
public partial class SettingsGeneralComponent : ComponentBase
{
    private UpscaleSettings settings = new();
        
    [Inject]
    private ISettingsService SettingsService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.settings = await this.SettingsService.LoadSettingsAsync<UpscaleSettings>();
    }

    private async Task SaveSettings()
    {
        await this.SettingsService.EnsurePath(this.settings.TempPath);
        await this.SettingsService.EnsurePath(Path.Combine(this.settings.TempPath, this.settings.ScaledPath));
        await this.SettingsService.EnsurePath(Path.Combine(this.settings.TempPath, this.settings.FramesPath));
        await this.SettingsService.SaveSettingsAsync(this.settings);
    }
}