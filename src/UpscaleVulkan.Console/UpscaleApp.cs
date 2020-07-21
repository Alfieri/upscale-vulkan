namespace UpscaleVulkan.Console
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class UpscaleApp
    {
        private readonly ILogger<UpscaleApp> _logger;
        private readonly IUpscaler _upscaler;
        private readonly IWaifu2x _waifu2X;
        private readonly IVideoConverter _videoConverter;

        public UpscaleApp(ILogger<UpscaleApp> logger, IUpscaler upscaler, IWaifu2x waifu2X, IVideoConverter videoConverter)
        {
            this._logger = logger;
            this._upscaler = upscaler;
            this._waifu2X = waifu2X;
            this._videoConverter = videoConverter;
        }
        
        public async Task Run(string[] args)
        {
            await this._upscaler.ExtractFrames(this._videoConverter);
            await this._upscaler.Upscale(this._waifu2X);
            //await this._upscaler.CreateVideoFromScaledFrames(this._videoConverter);
            //await this._upscaler.CreateFinaleVideo(this._videoConverter);
        }
    }
}