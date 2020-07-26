namespace UpscaleVulkan
{
    using System.IO;
    using System.Threading.Tasks;
    using Core;
    using Dtos;

    public class Upscaler : IUpscaler
    {
        private Video _video;
        
        private IntermediateVideo _intermediateVideo;

        public Upscaler(UpscaleSettings upscaleSettings)
        {
            this._video = new Video(new FileInfo(upscaleSettings.VideoFile));
            this._intermediateVideo = new IntermediateVideo(this._video);
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
            return this._intermediateVideo.CreateVideoFromUpscaledFrames(videoConverter);
        }

        public Task CreateFinaleVideo(IVideoConverter videoConverter)
        {
            return this._intermediateVideo.CreateFinaleVideo(videoConverter);
        }
    }
}