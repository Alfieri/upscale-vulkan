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

        internal Video()
        {
        }
        
        public Video(FileInfo videoFile)
        {
            this._videoFile = videoFile;
        }
        
        public FileInfo VideoFile => this._videoFile;

        public Video(FileInfo videoFile, List<Frame> frames)
        {
            this._videoFile = videoFile;
            this._frames = frames;
        }

        public virtual async Task ExtractFramesFromVideo(IVideoConverter videoConverter)
        {
            this._frames = await videoConverter.ExtractFrames(this);
        }

        public virtual async Task Upscale(IWaifu2x waifu2X)
        {
            IEnumerable<Frame> processableFrames = this._frames.Where(frame => !this.IsAlreadyUpscaled(frame)).OrderBy(f => f.FrameName);
            foreach (var frame in processableFrames)
            {
                ScaledFrame scaledFrame = await frame.Upscale(waifu2X);
                this._scaledFrames.Add(scaledFrame);
            }
        }

        public virtual async Task<IntermediateVideo> CreateVideoFromUpscaledFrames(IVideoConverter videoConverter)
        {
            return await videoConverter.CreateVideoFromFrames(this._framerate, this._scaledFrames);
        }

        public virtual Task CreateFinaleVideo(IVideoConverter videoConverter)
        {
            return videoConverter.CreateFinaleVideo(this);
        }

        private bool IsAlreadyUpscaled(Frame frame)
        {
            return frame.IsUpscaled;
        }
    }
}