﻿namespace UpscaleVulkan.Core
{
    using System.Collections.Generic;
    using System.Diagnostics;
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
            int processingIndex = 0;
            while (true)
            {
                if (processingIndex >= processableFrames.Count)
                {
                    break;
                }
                
                stopwatch.Start();
                Task t1 = Task.Run(() => this.SaveUpscaleFrame(waifu2X, processableFrames, processingIndex));
                processingIndex++;
                await Task.Delay(300);
                Task t2 = Task.Run(() => this.SaveUpscaleFrame(waifu2X, processableFrames, processingIndex));
                processingIndex++;
                await Task.Delay(300);
                Task t3 = Task.Run(() => this.SaveUpscaleFrame(waifu2X, processableFrames, processingIndex));
                processingIndex++;
                await Task.WhenAll(t1, t2);
                stopwatch.Stop();
                this._logger.LogInformation($"upscaling time: {stopwatch.ElapsedMilliseconds}ms");
                stopwatch.Reset();
            }

            var intermediateVideo = new IntermediateVideo(this);
            await intermediateVideo.CreateVideoFromUpscaledFrames(videoConverter, waifu2X.GetScaledPath());
            await intermediateVideo.CreateFinaleVideo(videoConverter);
        }

        private Task SaveUpscaleFrame(IWaifu2x waifu2X, List<Frame> processableFrames, int processingIndex)
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
}