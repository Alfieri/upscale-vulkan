namespace UpscaleVulkan
{
    using System.IO;
    using System.Threading.Tasks;
    using Core;
    using Dtos;

    public class Upscaler : IUpscaler
    {
        private readonly UpscaleSettings _upscaleSettings;

        private Video _video;

        public Upscaler(UpscaleSettings upscaleSettings)
        {
            this._upscaleSettings = upscaleSettings;
            this._video = new Video(new FileInfo(this._upscaleSettings.VideoFile));
        }

        public Task Upscale(IWaifu2x waifu2xImplementation)
        {
            return this._video.Upscale(waifu2xImplementation);
        }

        public Task ExtractFrames(IVideoConverter videoConverter)
        {
            return this._video.ExtractFramesFromVideo(videoConverter);
        }

        public Task CreateVideoFromScaledFrames(IVideoConverter videoConverter)
        {
            return this._video.CreateVideoFromUpscaledFrames(videoConverter);
        }

        public Task CreateFinaleVideo(IVideoConverter videoConverter)
        {
            return this._video.CreateFinaleVideo(videoConverter);
        }
    }
}