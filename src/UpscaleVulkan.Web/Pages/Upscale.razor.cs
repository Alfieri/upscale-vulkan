namespace UpscaleVulkan.Web.Pages
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Core;
    using Core.Settings;
    using Application.Services;
    
    public partial class Upscale
    {
        private UpscaleSettings upscaleSettings { get; set; } = new();

        private Video video { get; set; }
        
        [Inject]
        private ISettingsService settingsService { get; set; }
        
        [Inject]
        private IUpscaleService upscaleService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.upscaleSettings = await this.settingsService.LoadSettingsAsync<UpscaleSettings>();
            this.video = new Video(this.upscaleSettings.VideoFile);
        }

        private async Task StartUpscaling()
        {
            await this.upscaleService.Upscale(this.video);
        }
    }
}