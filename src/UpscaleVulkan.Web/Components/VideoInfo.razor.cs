namespace UpscaleVulkan.Web.Components
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using UpscaleVulkan.Application.Services;
    using Core;
    using Core.MediaInfo;
    
    public partial class VideoInfo : ComponentBase
    {
        private StreamInfo? videoStream;

        private string duration = string.Empty;

        private FfprobeJson videoInfo { get; set; } = new();
        
        [Inject]
        private IVideoConverter videoConverter { get; set; } 

        [Parameter]
        public Video Video { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.videoInfo = await this.videoConverter.GetVideoInfo(this.Video);
            var t = TimeSpan.FromMilliseconds(double.Parse(this.videoInfo.Format.DurationInMilliseconds ?? "0"));
            this.duration = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", 
                t.Hours, 
                t.Minutes, 
                t.Seconds, 
                t.Milliseconds);
            this.videoStream = this.videoInfo.Streams.FirstOrDefault(s => s.CodecType?.Equals(CodecType.Video) ?? false);
        }
    }
}