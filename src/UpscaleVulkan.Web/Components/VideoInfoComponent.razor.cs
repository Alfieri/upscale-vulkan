namespace UpscaleVulkan.Web.Components
{
    using System;
    using Microsoft.AspNetCore.Components;

    using Application.Helpers;
    using Core.MediaInfo;

    public partial class VideoInfoComponent : ComponentBase
    {
        private StreamInfo videoStream;

        private string duration = string.Empty;

        [Parameter]
        public FfprobeJson VideoInfo { get; set; } = new();

        protected override void OnParametersSet()
        {
            this.videoStream = this.VideoInfo.Streams.VideoStream();
            this.duration = this.GetHumanReadableDuration();
        }

        private string GetHumanReadableDuration()
        {
            var t = TimeSpan.FromMilliseconds(double.Parse(this.VideoInfo.Format.DurationInMilliseconds ?? "0"));
            return $"{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s:{t.Milliseconds:D3}ms";
        }
    }
}