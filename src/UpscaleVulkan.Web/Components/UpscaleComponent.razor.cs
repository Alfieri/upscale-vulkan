namespace UpscaleVulkan.Web.Components
{
    using System.Threading.Tasks;
    using Application;
    using Microsoft.AspNetCore.Components;
    using Core;
    using Core.Settings;
    using Application.Services;
    using Model;
    
    public partial class UpscaleComponent : ComponentBase
    {
        private ComponentState state = ComponentState.Error;
        
        private UpscaleSettings UpscaleSettings { get; set; } = new();

        private UpscaleContext Context { get; set; }

        [Inject]
        private ISettingsService SettingsService { get; set; }
        
        [Inject]
        private IStateService StateService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.UpscaleSettings = await this.SettingsService.LoadSettingsAsync<UpscaleSettings>();
            if (!string.IsNullOrEmpty(this.UpscaleSettings.VideoFile))
            {
                this.Context = new UpscaleContext(await this.StateService.GetCurrentState())
                {
                    Video = new Video(this.UpscaleSettings.VideoFile)
                };
                
                this.state = ComponentState.Content;
            }
        }

        private async Task StartUpscaling()
        {
            await this.Context.ProcessVideo();
        }
    }
}