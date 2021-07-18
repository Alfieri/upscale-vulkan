namespace UpscaleVulkan.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Core;
    using Core.MediaInfo;
    using Core.Settings;
    using Helpers;
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

        public async Task<FfprobeJson> GetVideoInfo(Video video)
        {
            await this.Initialize();
            ProcessResult processResult = await ProcessAsyncHelper.RunProcessAsync(this.ffmpegSettings.FfprobBin,
                $"-v quiet -print_format json -show_format -show_streams {video.VideoFile.FullName}", 5000);
            
            if (processResult.ExitCode > 0)
            {
                this.logger.LogError(processResult.Error);
                return new FfprobeJson();
            }

            return JsonSerializer.Deserialize<FfprobeJson>(processResult.Output);
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
                var argumentBuilder = new StringBuilder();
                argumentBuilder.Append($"{this.ffmpegSettings.HardwareAcceleration} ");
                argumentBuilder.Append($"-i \"{video.VideoFile.FullName}\" ");
                argumentBuilder.Append($"{this.ffmpegSettings.VideoToFramesPixFormat} ");
                argumentBuilder.Append($"\"{this.framesOutputDir}/%07d.png\"");
                await ProcessAsyncHelper.RunProcessAsync(this.ffmpegSettings.FfmpegBin, argumentBuilder.ToString());
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

        public async Task<FileInfo> CreateVideoFromFrames()
        {
            string scaledInputPath = Path.Combine(this.upscaleSettings.TempPath, this.upscaleSettings.ScaledPath);
            
            string intermediateVideo =
                Path.Combine(this.upscaleSettings.TempPath, this.ffmpegSettings.IntermediateVideoFile);

            var argumentBuilder = new StringBuilder();
            argumentBuilder.Append($"{this.ffmpegSettings.HardwareAcceleration} ");
            argumentBuilder.Append("-y ");
            argumentBuilder.Append($"-framerate {this.ffmpegSettings.Framerate} ");
            argumentBuilder.Append("-f image2 ");
            argumentBuilder.Append($"-i \"{scaledInputPath}/%07d.png\" ");
            argumentBuilder.Append($"-r {this.ffmpegSettings.Framerate} ");
            argumentBuilder.Append($"{this.ffmpegSettings.FramesToVideoPixFormat} ");
            argumentBuilder.Append($"{this.ffmpegSettings.Codec} ");
            argumentBuilder.Append($"{this.ffmpegSettings.Preset} ");
            argumentBuilder.Append($"{this.ffmpegSettings.AdditionalCodecParameter} ");
            argumentBuilder.Append($"\"{intermediateVideo}\"");

            await ProcessAsyncHelper.RunProcessAsync(this.ffmpegSettings.FfmpegBin, argumentBuilder.ToString());
            return new FileInfo(intermediateVideo);
        }

        public async Task CreateFinaleVideo(IntermediateVideo intermediateVideo)
        {
            string outputFile = Path.Combine(intermediateVideo.OriginalOriginalVideo.VideoFile.DirectoryName,
                $"{Path.GetFileNameWithoutExtension(intermediateVideo.OriginalOriginalVideo.VideoFile.Name)}_out.mp4");
            var argumentBuilder = new StringBuilder();
            argumentBuilder.Append($"{this.ffmpegSettings.HardwareAcceleration} ");
            argumentBuilder.Append("-y ");
            argumentBuilder.Append($"-i \"{intermediateVideo.IntermediateVideoFile?.FullName}\" ");
            argumentBuilder.Append($"-i \"{intermediateVideo.OriginalOriginalVideo.VideoFile.FullName}\" ");
            argumentBuilder.Append($"{this.ffmpegSettings.ConcatVideosParameter} ");
            argumentBuilder.Append($"\"{outputFile}\"");
            await ProcessAsyncHelper.RunProcessAsync(this.ffmpegSettings.FfmpegBin, argumentBuilder.ToString());
        }
    }
}