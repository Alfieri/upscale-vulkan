namespace UpscaleVulkan.Application.Services
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Core;
    using Core.Settings;
    using Exceptions;
    using Infrastructure;
    using Microsoft.Extensions.Logging;

    public class Waifu2xVulkan : IWaifu2x
    {
        private readonly ISettingsService settingsService;
        private readonly IFileProxy fileProxy;
        private readonly ILogger<Waifu2xVulkan> logger;

        private Waifu2xSettings? waifu2XSettings;
        private string? outputPath;

        public Waifu2xVulkan(ISettingsService settingsService, IFileProxy fileProxy, ILogger<Waifu2xVulkan> logger)
        {
            this.settingsService = settingsService;
            this.fileProxy = fileProxy;
            this.logger = logger;
        }

        public async Task Upscale(Frame frame)
        {
            await this.PrepareScaling();
            
            string outputFile = this.CreateScaledFrameFullName(frame);
            string inputFile = Path.Combine(frame.FramePath, frame.FrameName);
            
            var processStartInfo = this.CreateProcessStartInfo(inputFile, outputFile);
            this.logger.LogInformation($"{processStartInfo.FileName} {processStartInfo.Arguments}");
            do
            {
                var process = new Process {StartInfo = processStartInfo};
                process.ErrorDataReceived +=
                    (sender, args) => this.logger.LogError($"Waifu2x Vulkan output: {args.Data}");
                process.OutputDataReceived +=
                    (sender, args) => this.logger.LogDebug($"Waifu2x Vulkan output: {args.Data}");

                process.Start();
                if (process == null)
                {
                    throw new ScalingFailedException("could not start Waifu2x Vulkan.");
                }

                process.WaitForExit();
                if (process.ExitCode > 0)
                {
                    process.BeginErrorReadLine();
                }

                process.BeginOutputReadLine();
            } while (!await this.fileProxy.ExistsAsync(outputFile));

        }

        private async Task PrepareScaling()
        {
            this.waifu2XSettings = await this.settingsService.LoadSettingsAsync<Waifu2xSettings>();
            var upscaleSettings = await this.settingsService.LoadSettingsAsync<UpscaleSettings>();
            this.outputPath = Path.Combine(upscaleSettings.TempPath, upscaleSettings.ScaledPath);
        }

        public Task<bool> IsAlreadyUpscaled(Frame frame)
        {
            return this.fileProxy.ExistsAsync(this.CreateScaledFrameFullName(frame));
        }

        public string GetScaledPath()
        {
            return this.outputPath;
        }

        private ProcessStartInfo CreateProcessStartInfo(string inputFile, string outputFile)
        {
            var processStartInfo = new ProcessStartInfo("bash")
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = this.waifu2XSettings.WorkingDir,
                Arguments = $"-c \"{this.waifu2XSettings.Executable} "
            };

            this.SetArgumentsString(processStartInfo, this.waifu2XSettings);
            processStartInfo.Arguments += $"-i {inputFile} ";
            processStartInfo.Arguments += $"-o {outputFile}";
            processStartInfo.Arguments += "\"";
            
            return processStartInfo;
        }

        private void SetArgumentsString(ProcessStartInfo processStartInfo, Waifu2xSettings waifu2XSettings)
        {
            if (!string.IsNullOrEmpty(waifu2XSettings.ModelPath))
            {
                processStartInfo.Arguments += $"-m {waifu2XSettings.ModelPath} ";
            }

            if (waifu2XSettings.Scale > 0)
            {
                processStartInfo.Arguments += $"-s {waifu2XSettings.Scale} ";
            }

            if (waifu2XSettings.NoiseLevel > 0)
            {
                processStartInfo.Arguments += $"-n {waifu2XSettings.NoiseLevel} ";
            }
        }

        private string CreateScaledFrameFullName(Frame frame)
        {
            return Path.Combine(this.outputPath, frame.FrameName);
        }
    }
}