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
                ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.HardwareAcceleration);
                ffmpegProcessStartInfo.ArgumentList.Add($"-i {video.VideoFile.FullName}");
                ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.VideoToFramesPixFormat);
                ffmpegProcessStartInfo.ArgumentList.Add($"{this._framesOutputDir}/%07d.png");
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

        public Task<FileInfo> CreateVideoFromFrames(double framerate, string scaledInputPath)
        {
            string intermediateVideo =
                Path.Combine(this._upscaleSettings.TempPath, this._ffmpegSettings.IntermediateVideoFile);
            
            var ffmpegProcessStartInfo = new ProcessStartInfo(this._ffmpegSettings.FfmpegBin);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.HardwareAcceleration);
            ffmpegProcessStartInfo.ArgumentList.Add("-y");
            ffmpegProcessStartInfo.ArgumentList.Add($"-framerate {framerate}");
            ffmpegProcessStartInfo.ArgumentList.Add("-f image2");
            ffmpegProcessStartInfo.ArgumentList.Add($"-i {scaledInputPath}");
            ffmpegProcessStartInfo.ArgumentList.Add($"-r {framerate}");
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.FramesToVideoPixFormat);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.Codec);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.Preset);
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.AdditionalCodecParameter);
            ffmpegProcessStartInfo.ArgumentList.Add(intermediateVideo);

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
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.HardwareAcceleration);
            ffmpegProcessStartInfo.ArgumentList.Add("-y");
            ffmpegProcessStartInfo.ArgumentList.Add($"-i {intermediateVideo.IntermediateVideoFile?.FullName}");
            ffmpegProcessStartInfo.ArgumentList.Add($"-i {intermediateVideo.OriginalVideo.VideoFile.FullName}");
            ffmpegProcessStartInfo.ArgumentList.Add(this._ffmpegSettings.ConcatVideosParameter);
            ffmpegProcessStartInfo.ArgumentList.Add(outputFile);
            
            return Task.Run(() =>
            {
                var process = Process.Start(ffmpegProcessStartInfo);
                process?.WaitForExit();
            });
        }
    }
}