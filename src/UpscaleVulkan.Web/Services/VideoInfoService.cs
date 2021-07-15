namespace UpscaleVulkan.Web.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.Services;
    using Core;
    using Core.MediaInfo;

    public class VideoInfoService : IVideoInfoService
    {
        private readonly IVideoConverter videoConverter;

        public VideoInfoService(IVideoConverter videoConverter)
        {
            this.videoConverter = videoConverter;
        }

        public async Task<FfprobeJson> GetVideoInfo(Video video)
        {
            return await this.videoConverter.GetVideoInfo(video);
        }

        public string GetHumanReadableDuration(FfprobeJson mediaInfo)
        {
            var t = TimeSpan.FromMilliseconds(double.Parse(mediaInfo.Format.DurationInMilliseconds ?? "0"));
            return $"{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s:{t.Milliseconds:D3}ms";
        }
    }
}