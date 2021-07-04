namespace UpscaleVulkan.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.Extensions.Logging;
    using Settings;

    public class Video
    {
        private readonly ILogger<Video> _logger;
        private readonly FileInfo _videoFile;

        private List<Frame> _frames = new List<Frame>();

        private bool _resume;

        public Video(UpscaleSettings upscaleSettings, ILogger<Video> logger)
        {
            this._logger = logger;
            this._videoFile = new FileInfo(upscaleSettings.VideoFile);
            this._resume = upscaleSettings.Resume;
        }
        
        public FileInfo VideoFile => this._videoFile;

        public event EventHandler? ScalingStarted;

        public event EventHandler<ScaleReportingEventArgs>? ScalingFinished;

        public async Task Upscale(IWaifu2x waifu2X, IVideoConverter videoConverter)
        {
            if (this._resume)
            {
                await this.ReadFrames(waifu2X, videoConverter);
            }
            else
            {
                List<Frame> extractFrames = await videoConverter.ExtractFrames(this);
                this._frames.AddRange(extractFrames);
            }

            while (this.IsUpscaleFrameAvailable())
            {
                await this.UpscaleFrames(waifu2X);
                
                // read frames again because sometimes upscale does not work
                await this.ReadFrames(waifu2X, videoConverter);
            }

            var intermediateVideo = new IntermediateVideo(this);
            await intermediateVideo.CreateVideoFromUpscaledFrames(videoConverter, waifu2X.GetScaledPath());
            await intermediateVideo.CreateFinaleVideo(videoConverter);
        }

        private async Task ReadFrames(IWaifu2x waifu2X, IVideoConverter videoConverter)
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

        private bool IsUpscaleFrameAvailable()
        {
            return this._frames.Count(f => f.IsUpscaled == false) > 0;
        }

        private async Task UpscaleFrames(IWaifu2x waifu2X)
        {
            List<Frame> processableFrames = this._frames.Where(f => f.IsUpscaled == false).OrderBy(f => f.FrameName).ToList();
            int processingIndex = 0;
            while (true)
            {
                if (processingIndex >= processableFrames.Count)
                {
                    break;
                }

                this.OnStartScaling();
                Task t1 = Task.Run(() => this.UpscaleOneFrame(waifu2X, processableFrames, processingIndex));
                processingIndex++;
                await Task.Delay(200);
                Task t2 = Task.Run(() => this.UpscaleOneFrame(waifu2X, processableFrames, processingIndex));
                processingIndex++;
                await Task.Delay(400);
                Task t3 = Task.Run(() => this.UpscaleOneFrame(waifu2X, processableFrames, processingIndex));
                processingIndex++;
                await Task.Delay(200);
                Task t4 = Task.Run(() => this.UpscaleOneFrame(waifu2X, processableFrames, processingIndex));
                processingIndex++;
                await Task.WhenAll(t1, t2, t3, t4);
                this.OnScalingFinished(4, processableFrames.Count, processingIndex);
            }
        }

        private void OnScalingFinished(in int batchSize, in int numberOfFrames, int currentFrame)
        {
            this.ScalingFinished?.Invoke(this, new ScaleReportingEventArgs(batchSize, numberOfFrames, currentFrame));
        }

        private void OnStartScaling()
        {
            this.ScalingStarted?.Invoke(this, EventArgs.Empty);
        }

        private Task UpscaleOneFrame(IWaifu2x waifu2X, List<Frame> processableFrames, int processingIndex)
        {
            try
            {
                Frame nextFrame = this.SaveGetNextFrame(processableFrames, processingIndex);
                return waifu2X.Upscale(nextFrame);
            }
            catch (InvalidFrameAccessException)
            {
                this._logger.LogInformation("Upscaling done");
            }

            return Task.CompletedTask;
        }

        private Frame SaveGetNextFrame(List<Frame> processableFrames, in int processingIndex)
        {
            if (processingIndex >= processableFrames.Count)
            {
                throw new InvalidFrameAccessException();
            }

            return processableFrames[processingIndex];
        }
    }

    public class ScaleReportingEventArgs
    {
        public ScaleReportingEventArgs(int batchSize, int numberOfFrames, int currentFrame)
        {
            this.BatchSize = batchSize;
            this.NumberOfFrames = numberOfFrames;
            this.CurrentFrame = currentFrame;
        }
        
        public int BatchSize { get; }

        public int NumberOfFrames { get; }
        
        public int CurrentFrame { get; }
    }
}