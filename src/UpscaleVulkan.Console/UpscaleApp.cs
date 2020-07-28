namespace UpscaleVulkan.Console
{
    using System.Threading.Tasks;
    using Core;
    using Core.Settings;
    using Microsoft.Extensions.Logging;

    public class UpscaleApp
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IWaifu2x _waifu2X;
        private readonly IVideoConverter _videoConverter;
        private readonly UpscaleSettings _upscaleSettings;

        public UpscaleApp(ILoggerFactory loggerFactory, IWaifu2x waifu2X, IVideoConverter videoConverter, UpscaleSettings upscaleSettings)
        {
            this._loggerFactory = loggerFactory;
            this._waifu2X = waifu2X;
            this._videoConverter = videoConverter;
            this._upscaleSettings = upscaleSettings;
        }
        
        public async Task Run(string[] args)
        {
            var video = new Video(this._upscaleSettings, this._loggerFactory.CreateLogger<Video>());
            await video.Upscale(this._waifu2X, this._videoConverter);
        }
    }
}