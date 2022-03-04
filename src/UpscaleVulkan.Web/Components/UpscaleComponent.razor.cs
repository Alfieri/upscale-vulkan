namespace UpscaleVulkan.Web.Components;

using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
    
using Application;
using Application.Services;
using Core;
using Core.MediaInfo;
using Core.Settings;
using Model;
using Services;
    
public partial class UpscaleComponent : ComponentBase
{
    private ComponentState state = ComponentState.Error;
        
    private Video Video { get; set; }

    private FfprobeJson VideoInfo { get; set; }

    [Inject]
    private IUpscaleComponentViewService UpscaleComponentViewService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await this.UpscaleComponentViewService.InitializeAsync();
        this.Video = this.UpscaleComponentViewService.GetVideo();
        this.VideoInfo = this.UpscaleComponentViewService.GetVideoInfo();

        if (this.Video is not null)
        {
            this.state = ComponentState.Content;
        }
    }

    private async Task StartUpscaling()
    {
        await this.UpscaleComponentViewService.ProcessVideo();
    }
}