namespace UpscaleVulkan.Application.Services
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Core.Settings;
    using Microsoft.Extensions.Logging;

    public class FfmpegService : IVideoConverter
    {
        private readonly ISettingsService settingsService;
        private readonly ILogger<FfmpegService> logger;
        private string framesOutputDir;
        private FfmpegSettings ffmpegSettings;
        private UpscaleSettings upscaleSettings;

        public FfmpegService(ISettingsService settingsService, ILogger<FfmpegService> logger)
        {
            this.settingsService = settingsService;
            this.logger = logger;
        }

        public async Task<List<Frame>> ExtractFrames(Video video)
        {
            await this.Initialize();
            
            if (this.FramesOutputDirIsNotEmpty())
            {
                this.logger.LogInformation($"Frames directory is not empty. Delete existing frames first");
                var directoryInfo = new DirectoryInfo(this.framesOutputDir);
                foreach (FileInfo file in directoryInfo.EnumerateFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                var ffmpegProcessStartInfo = new ProcessStartInfo(this.ffmpegSettings.FfmpegBin);
                ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.HardwareAcceleration} ";
                ffmpegProcessStartInfo.Arguments += $"-i \"{video.VideoFile.FullName}\" ";
                ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.VideoToFramesPixFormat} ";
                ffmpegProcessStartInfo.Arguments += $"\"{this.framesOutputDir}/%07d.png\"";
                Process ffmpegProcess = Process.Start(ffmpegProcessStartInfo);
                ffmpegProcess?.WaitForExit();  
            }

            return this.GetFrames();
        }

        public List<Frame> GetFrames()
        {
            return Directory.EnumerateFiles(this.framesOutputDir).Select(frame =>
            {
                var frameFile = new FileInfo(frame);
                return new Frame(frameFile.DirectoryName, frameFile.Name);
            }).OrderBy(f => f.FrameName).ToList();
        }

        private async Task Initialize()
        {
            this.ffmpegSettings = await this.settingsService.LoadSettingsAsync<FfmpegSettings>();
            this.upscaleSettings = await this.settingsService.LoadSettingsAsync<UpscaleSettings>();
            this.framesOutputDir = Path.Combine(this.upscaleSettings.TempPath, this.upscaleSettings.FramesPath);
            if (!Directory.Exists(this.framesOutputDir))
            {
                Directory.CreateDirectory(this.framesOutputDir);
            }
        }

        private bool FramesOutputDirIsNotEmpty()
        {
            return Directory.GetFiles(this.framesOutputDir).Length > 0;
        }

        public Task<FileInfo> CreateVideoFromFrames()
        {
            string scaledInputPath = Path.Combine(this.upscaleSettings.TempPath, this.upscaleSettings.ScaledPath);
            
            string intermediateVideo =
                Path.Combine(this.upscaleSettings.TempPath, this.ffmpegSettings.IntermediateVideoFile);
            
            var ffmpegProcessStartInfo = new ProcessStartInfo(this.ffmpegSettings.FfmpegBin);
            ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.HardwareAcceleration} ";
            ffmpegProcessStartInfo.Arguments += "-y ";
            ffmpegProcessStartInfo.Arguments += $"-framerate {this.ffmpegSettings.Framerate} ";
            ffmpegProcessStartInfo.Arguments += "-f image2 ";
            ffmpegProcessStartInfo.Arguments += $"-i \"{scaledInputPath}/%07d.png\" ";
            ffmpegProcessStartInfo.Arguments += $"-r {this.ffmpegSettings.Framerate} ";
            ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.FramesToVideoPixFormat} ";
            ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.Codec} ";
            ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.Preset} ";
            ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.AdditionalCodecParameter} ";
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
            string outputFile = Path.Combine(intermediateVideo.OriginalOriginalVideo.VideoFile.DirectoryName,
                $"{Path.GetFileNameWithoutExtension(intermediateVideo.OriginalOriginalVideo.VideoFile.Name)}_out.mp4");
            var ffmpegProcessStartInfo = new ProcessStartInfo(this.ffmpegSettings.FfmpegBin);
            ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.HardwareAcceleration} ";
            ffmpegProcessStartInfo.Arguments += "-y ";
            ffmpegProcessStartInfo.Arguments += $"-i \"{intermediateVideo.IntermediateVideoFile?.FullName}\" ";
            ffmpegProcessStartInfo.Arguments += $"-i \"{intermediateVideo.OriginalOriginalVideo.VideoFile.FullName}\" ";
            ffmpegProcessStartInfo.Arguments += $"{this.ffmpegSettings.ConcatVideosParameter} ";
            ffmpegProcessStartInfo.Arguments += $"\"{outputFile}\"";
            
            return Task.Run(() =>
            {
                var process = Process.Start(ffmpegProcessStartInfo);
                process?.WaitForExit();
            });
        }
    }
}