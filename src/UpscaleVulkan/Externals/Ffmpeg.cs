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

        private void CreateFramesOutputDirectory()
        {
            this._framesOutputDir = Path.Combine(this._ffmpegParameter.TempPath, "frames");
            if (!Directory.Exists(this._framesOutputDir))
            {
                Directory.CreateDirectory(this._framesOutputDir);
            }
        }

        private bool FramesOutputDirIsNotEmpty()
        {
            return Directory.GetFiles(this._framesOutputDir).Length > 0;
        }

        public Task<IntermediateVideo> CreateVideoFromFrames(double framerate, string scaledInputPath)
        {
            string intermediateVideo =
                Path.Combine(this._ffmpegParameter.TempPath, this._ffmpegParameter.IntermediateVideoFile);
            
            var ffmpegProcessStartInfo = new ProcessStartInfo(this._ffmpegParameter.FfmpegBin);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.HardwareAcceleration);
            ffmpegProcessStartInfo.ArgumentList.Add("-y");
            ffmpegProcessStartInfo.ArgumentList.Add($"-framerate {framerate}");
            ffmpegProcessStartInfo.ArgumentList.Add("-f image2");
            ffmpegProcessStartInfo.ArgumentList.Add($"-i {scaledInputPath}");
            ffmpegProcessStartInfo.ArgumentList.Add($"-r {framerate}");
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.FramesToVideoPixFormat);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.Codec);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.Preset);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.AdditionalCodecParameter);
            ffmpegProcessStartInfo.ArgumentList.Add(intermediateVideo);

            return Task.Run(() =>
            {
                var process = Process.Start(ffmpegProcessStartInfo);
                process?.WaitForExit();
                return new IntermediateVideo(new FileInfo(intermediateVideo));
            });
        }

        public Task CreateFinaleVideo(Video video)
        {
            string outputFile = Path.Combine(video.VideoFile.DirectoryName,
                $"{Path.GetFileNameWithoutExtension(video.VideoFile.Name)}_out.mp4");
            var ffmpegProcessStartInfo = new ProcessStartInfo(this._ffmpegParameter.FfmpegBin);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.HardwareAcceleration);
            ffmpegProcessStartInfo.ArgumentList.Add("-y");
            ffmpegProcessStartInfo.ArgumentList.Add($"-i {video.IntermediateVideo.IntermediateVideoFile.FullName}");
            ffmpegProcessStartInfo.ArgumentList.Add($"-i {video.VideoFile.FullName}");
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegParameter.ConcatVideosParameter);
            ffmpegProcessStartInfo.ArgumentList.Add(outputFile);
            
            return Task.Run(() =>
            {
                var process = Process.Start(ffmpegProcessStartInfo);
                process?.WaitForExit();
            });
        }
    }
}