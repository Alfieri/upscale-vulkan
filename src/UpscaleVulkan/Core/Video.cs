namespace UpscaleVulkan.Core
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Settings;

    public class Video
    {
        private readonly ILogger<Video> _logger;
        private readonly FileInfo _videoFile;

        private double _framerate = 29.97;

        private List<Frame> _frames = new List<Frame>();

        private readonly List<ScaledFrame> _scaledFrames = new List<ScaledFrame>();
        
        private bool _resume;

        public Video(UpscaleSettings upscaleSettings, ILogger<Video> logger)
        {
            this._logger = logger;
            this._videoFile = new FileInfo(upscaleSettings.VideoFile);
            this._resume = upscaleSettings.Resume;
        }
        
        public FileInfo VideoFile => this._videoFile;

        public List<ScaledFrame> ScaledFrames => this._scaledFrames;

        public double Framerate => this._framerate;

        public async Task Upscale(IWaifu2x waifu2X, IVideoConverter videoConverter)
        {
            if (this._resume)
            {
                this._logger.LogInformation("Resume from previous upscaling");
                this._frames.AddRange(videoConverter.GetFrames());
                foreach (Frame frame in this._frames)
                {
                    if (!await waifu2X.IsAlreadyUpscaled(frame)) continue;
                    this._logger.LogInformation($"{frame} already scaled");
                    frame.SetToUpscaled();
                }
            }
            else
            {
                List<Frame> extractFrames = await videoConverter.ExtractFrames(this);
                this._frames.AddRange(extractFrames);
            }

            var stopwatch = new Stopwatch();
            List<Frame> processableFrames = this._frames.Where(f => f.IsUpscaled == false).OrderBy(f => f.FrameName).ToList();
            for (int index = 0; index < processableFrames.Count; index++)
            {
                var f1 = processableFrames[index];
                index++;
                var f2 = processableFrames[index];
                stopwatch.Start();
                Task<ScaledFrame> t1 = Task.Run(() => waifu2X.Upscale(f1));
                await Task.Delay(300);
                Task<ScaledFrame> t2 = Task.Run(() => waifu2X.Upscale(f2));
                
                await Task.WhenAll(t1, t2);
                stopwatch.Stop();
                this._logger.LogInformation($"upscaling time: {stopwatch.ElapsedMilliseconds}ms");
                stopwatch.Reset();
                this.ScaledFrames.Add(t1.Result);
                this.ScaledFrames.Add(t2.Result);
            }

            var intermediateVideo = new IntermediateVideo(this);
            await intermediateVideo.CreateVideoFromUpscaledFrames(videoConverter);
            await intermediateVideo.CreateFinaleVideo(videoConverter);
        }
    }
}