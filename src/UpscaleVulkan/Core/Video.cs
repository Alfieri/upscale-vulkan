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

        private List<Frame> _frames = new List<Frame>();

        private readonly List<ScaledFrame> _scaledFrames = new List<ScaledFrame>();
        
        private IntermediateVideo intermediateVideo;

        public Video(FileInfo videoFile)
        {
            this._videoFile = videoFile;
        }
        
        public FileInfo VideoFile => this._videoFile;

        public IntermediateVideo IntermediateVideo => this.intermediateVideo;

        public Video(FileInfo videoFile, List<Frame> frames)
        {
            this._videoFile = videoFile;
            this._frames = frames;
        }

        public async Task ExtractFramesFromVideo(IVideoConverter videoConverter)
        {
            List<Frame> extractFrames = await videoConverter.ExtractFrames(this);
            this._frames.AddRange(extractFrames);
        }

        public async Task Upscale(IWaifu2x waifu2X)
        {
            IEnumerable<Frame> processableFrames = this._frames.Where(frame => !this.IsAlreadyUpscaled(frame)).OrderBy(f => f.FrameName);
            foreach (var frame in processableFrames)
            {
                ScaledFrame scaledFrame = await frame.Upscale(waifu2X);
                this._scaledFrames.Add(scaledFrame);
            }
        }

        public async Task CreateVideoFromUpscaledFrames(IVideoConverter videoConverter)
        {
            if (this._scaledFrames.Count <= 0)
            {
                return;
            }
            
            string scaledPath = this._scaledFrames[0].FramePath; 
            this.intermediateVideo = await videoConverter.CreateVideoFromFrames(this._framerate, scaledPath);
        }

        public Task CreateFinaleVideo(IVideoConverter videoConverter)
        {
            return videoConverter.CreateFinaleVideo(this);
        }

        private bool IsAlreadyUpscaled(Frame frame)
        {
            return frame.IsUpscaled;
        }
    }
}