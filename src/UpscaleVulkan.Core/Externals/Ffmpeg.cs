namespace UpscaleVulkan.Externals
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Core.Settings;
    using Microsoft.Extensions.Logging;

    public class Ffmpeg : IVideoConverter
    {
        private readonly UpscaleSettings _upscaleSettings;
        private readonly FfmpegSettings _ffmpegSettings;
        private readonly ILogger<Ffmpeg> _logger;
        private string _framesOutputDir;

        public Ffmpeg(UpscaleSettings upscaleSettings, FfmpegSettings ffmpegSettings, ILogger<Ffmpeg> logger)
        {
            this._upscaleSettings = upscaleSettings;
            this._ffmpegSettings = ffmpegSettings;
            this._logger = logger;
            this._framesOutputDir = Path.Combine(this._upscaleSettings.TempPath, this._ffmpegSettings.FramesPath);
            if (!Directory.Exists(this._framesOutputDir))
            {
                Directory.CreateDirectory(this._framesOutputDir);
            }
        }

        public Task<List<Frame>> ExtractFrames(Video video)
        {
            if (this.FramesOutputDirIsNotEmpty())
            {
                this._logger.LogInformation($"Frames directory is not empty. Delete existing frames first");
                var directoryInfo = new DirectoryInfo(this._framesOutputDir);
                foreach (FileInfo file in directoryInfo.EnumerateFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                var ffmpegProcessStartInfo = new ProcessStartInfo(this._ffmpegSettings.FfmpegBin);
                ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.HardwareAcceleration} ";
                ffmpegProcessStartInfo.Arguments += $"-i \"{video.VideoFile.FullName}\" ";
                ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.VideoToFramesPixFormat} ";
                ffmpegProcessStartInfo.Arguments += $"\"{this._framesOutputDir}/%07d.png\"";
                Process ffmpegProcess = Process.Start(ffmpegProcessStartInfo);
                ffmpegProcess?.WaitForExit();  
            }
            
            return Task.Run(() =>
            {
                return this.GetFrames();
            });
        }

        public List<Frame> GetFrames()
        {
            return Directory.EnumerateFiles(this._framesOutputDir).Select(frame =>
            {
                var frameFile = new FileInfo(frame);
                return new Frame(frameFile.DirectoryName, frameFile.Name);
            }).OrderBy(f => f.FrameName).ToList();
        }

        private bool FramesOutputDirIsNotEmpty()
        {
            return Directory.GetFiles(this._framesOutputDir).Length > 0;
        }

        public Task<FileInfo> CreateVideoFromFrames(string scaledInputPath)
        {
            string intermediateVideo =
                Path.Combine(this._upscaleSettings.TempPath, this._ffmpegSettings.IntermediateVideoFile);
            
            var ffmpegProcessStartInfo = new ProcessStartInfo(this._ffmpegSettings.FfmpegBin);
            ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.HardwareAcceleration} ";
            ffmpegProcessStartInfo.Arguments += "-y ";
            ffmpegProcessStartInfo.Arguments += $"-framerate {this._ffmpegSettings.Framerate} ";
            ffmpegProcessStartInfo.Arguments += "-f image2 ";
            ffmpegProcessStartInfo.Arguments += $"-i \"{scaledInputPath}/%07d.png\" ";
            ffmpegProcessStartInfo.Arguments += $"-r {this._ffmpegSettings.Framerate} ";
            ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.FramesToVideoPixFormat} ";
            ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.Codec} ";
            ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.Preset} ";
            ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.AdditionalCodecParameter} ";
            ffmpegProcessStartInfo.Arguments += $"\"{intermediateVideo}\"";

            return Task.Run(() =>
            {
                var process = Process.Start(ffmpegProcessStartInfo);
                process?.WaitForExit();
                return new FileInfo(intermediateVideo);
            });
        }

        public Task CreateFinaleVideo(IntermediateVideo intermediateVideo)
        {
            string outputFile = Path.Combine(intermediateVideo.OriginalVideo.VideoFile.DirectoryName,
                $"{Path.GetFileNameWithoutExtension(intermediateVideo.OriginalVideo.VideoFile.Name)}_out.mp4");
            var ffmpegProcessStartInfo = new ProcessStartInfo(this._ffmpegSettings.FfmpegBin);
            ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.HardwareAcceleration} ";
            ffmpegProcessStartInfo.Arguments += "-y ";
            ffmpegProcessStartInfo.Arguments += $"-i \"{intermediateVideo.IntermediateVideoFile?.FullName}\" ";
            ffmpegProcessStartInfo.Arguments += $"-i \"{intermediateVideo.OriginalVideo.VideoFile.FullName}\" ";
            ffmpegProcessStartInfo.Arguments += $"{this._ffmpegSettings.ConcatVideosParameter} ";
            ffmpegProcessStartInfo.Arguments += $"\"{outputFile}\"";
            
            return Task.Run(() =>
            {
                var process = Process.Start(ffmpegProcessStartInfo);
                process?.WaitForExit();
            });
        }
    }
}