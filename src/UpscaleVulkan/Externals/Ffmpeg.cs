namespace UpscaleVulkan.Externals
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Dtos;
    using Microsoft.Extensions.Logging;

    public class Ffmpeg : IVideoConverter
    {
        private readonly FfmpegParameter _ffmpegParameter;
        private readonly ILogger<Ffmpeg> _logger;
        private string _framesOutputDir;

        public Ffmpeg(FfmpegParameter ffmpegParameter, ILogger<Ffmpeg> logger)
        {
            this._ffmpegParameter = ffmpegParameter;
            this._logger = logger;
            this.CreateFramesOutputDirectory();
        }

        private void CreateFramesOutputDirectory()
        {
            this._framesOutputDir = Path.Combine(this._ffmpegParameter.TempPath, "frames");
            if (!Directory.Exists(this._framesOutputDir))
            {
                Directory.CreateDirectory(this._framesOutputDir);
            }
        }

        public Task<List<Frame>> ExtractFrames(Video video)
        {
            if (this.FramesOutputDirIsNotEmpty())
            {
                this._logger.LogInformation($"Frames directory is not empty. Use existing frames.");
            }
            else
            {
                var ffmpegProcessStartInfo = new ProcessStartInfo(this._ffmpegParameter.FfmpegBin);
                ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.HardwareAcceleration);
                ffmpegProcessStartInfo.ArgumentList.Add($"-i {video.VideoFile.FullName}");
                ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.VideoToFramesPixFormat);
                ffmpegProcessStartInfo.ArgumentList.Add($"{this._framesOutputDir}/%07d.png");
                Process ffmpegProcess = Process.Start(ffmpegProcessStartInfo);
                ffmpegProcess?.WaitForExit();  
            }
            
            return Task.Run(() =>
            {
                return Directory.GetFiles(this._framesOutputDir).Select(frame =>
                {
                    var frameFile = new FileInfo(frame);
                    return new Frame(frameFile.DirectoryName, frameFile.Name);
                }).ToList();
            });
        }

        private bool FramesOutputDirIsNotEmpty()
        {
            return Directory.GetFiles(this._framesOutputDir).Length > 0;
        }

        public Task<IntermediateVideo> CreateVideoFromFrames(double framerate, List<ScaledFrame> scaledFrames)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateFinaleVideo(Video video)
        {
            throw new System.NotImplementedException();
        }
    }
}