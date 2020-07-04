namespace UpscaleVulkan.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class Video
    {
        private readonly FileInfo _videoFile;

        private double _framerate = 29.97;

        private List<Frame> _frames;

        private List<ScaledFrame> _scaledFrames = new List<ScaledFrame>();

        public Video(FileInfo videoFile)
        {
            this._videoFile = videoFile;
        }

        public Video(FileInfo videoFile, List<Frame> frames)
        {
            this._videoFile = videoFile;
            this._frames = frames;
        }

        public async Task ExtractFramesFromVideo(IVideoConverter videoConverter)
        {
            this._frames = await videoConverter.ExtractFrames();
        }

        public async Task Upscale(IWaifu2x waifu2X)
        {
            IEnumerable<Frame> processableFrames = this._frames.Where(frame => !this.IsAlreadyUpscaled(frame));
            foreach (var frame in processableFrames)
            {
                ScaledFrame scaledFrame = await frame.Upscale(waifu2X);
                this._scaledFrames.Add(scaledFrame);
            }
        }

        public async Task<IntermediateVideo> CreateVideoFromUpscaledFrames(IVideoConverter videoConverter)
        {
            return await videoConverter.CreateVideoFromFrames(this._framerate, this._scaledFrames);
        }

        private bool IsAlreadyUpscaled(Frame frame)
        {
            return frame.IsUpscaled;
        }
    }
}