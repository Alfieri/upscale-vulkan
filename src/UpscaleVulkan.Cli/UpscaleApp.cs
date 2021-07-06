namespace UpscaleVulkan.Cli
{
    using System.Threading.Tasks;
    using Core;
    using Core.Settings;
    using Microsoft.Extensions.Logging;
    using Reporting;

    public class UpscaleApp
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IWaifu2x waifu2X;
        private readonly IVideoConverter videoConverter;
        private readonly UpscaleSettings upscaleSettings;

        public UpscaleApp(ILoggerFactory loggerFactory, IWaifu2x waifu2X, IVideoConverter videoConverter, UpscaleSettings upscaleSettings)
        {
            this.loggerFactory = loggerFactory;
            this.waifu2X = waifu2X;
            this.videoConverter = videoConverter;
            this.upscaleSettings = upscaleSettings;
        }
        
        public async Task Run(string[] args)
        {
            var video = new Video(this.upscaleSettings, this.loggerFactory.CreateLogger<Video>());
            var timeEstimation = new TimeEstimation(video, this.loggerFactory.CreateLogger<TimeEstimation>());
            await video.Upscale(this.waifu2X, this.videoConverter);
        }
    }
}