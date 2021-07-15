namespace UpscaleVulkan.Web.Components
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Core;
    using Core.Settings;
    using Application.Services;
    using Model;
    
    public partial class UpscaleComponent : ComponentBase
    {
        private ComponentState state = ComponentState.Error;
        
        private UpscaleSettings upscaleSettings { get; set; } = new();

        private Video video { get; set; }
        
        [Inject]
        private ISettingsService settingsService { get; set; }
        
        [Inject]
        private IUpscaleService upscaleService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.upscaleSettings = await this.settingsService.LoadSettingsAsync<UpscaleSettings>();
            if (!string.IsNullOrEmpty(this.upscaleSettings.VideoFile))
            {
                this.video = new Video(this.upscaleSettings.VideoFile);
                this.state = ComponentState.Content;
            }
        }

        private async Task StartUpscaling()
        {
            await this.upscaleService.Upscale(this.video);
        }
    }
}