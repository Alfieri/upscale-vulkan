namespace UpscaleVulkan.Web.Components
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using UpscaleVulkan.Application.Services;
    using Core;
    using Core.MediaInfo;
    using Services;
    
    public partial class VideoInfo : ComponentBase
    {
        private StreamInfo? videoStream;

        private string duration = string.Empty;

        private FfprobeJson videoInfo { get; set; } = new();
        
        [Inject]
        private IVideoInfoService VideoInfoService { get; set; } 

        [Parameter]
        public Video Video { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.videoInfo = await this.VideoInfoService.GetVideoInfo(this.Video);
            this.videoStream = this.videoInfo.Streams.FirstOrDefault(s => s.CodecType?.Equals(CodecType.Video) ?? false);
        }
    }
}