namespace UpscaleVulkan.Externals
{
    using System;
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
        private readonly Waifu2xSettings _waifu2XSettings;
        
        private readonly IFileProxy _fileProxy;
        
        private readonly ILogger<Waifu2xVulkan> _logger;
        
        private string _outputPath;

        public Waifu2xVulkan(UpscaleSettings upscaleSettings, Waifu2xSettings waifu2XSettings, IFileProxy fileProxy, ILogger<Waifu2xVulkan> logger)
        {
            this._waifu2XSettings = waifu2XSettings;
            this._fileProxy = fileProxy;
            this._logger = logger;
            if (string.IsNullOrEmpty(waifu2XSettings.OutputPath))
            {
                throw new ArgumentNullException(nameof(waifu2XSettings.OutputPath));
            }
            
            this._outputPath = Path.Combine(upscaleSettings.TempPath, waifu2XSettings.OutputPath);
        }

        public async Task Upscale(Frame frame)
        {
                string outputFile = this.CreateScaledFrameFullName(frame);
                string inputFile = Path.Combine(frame.FramePath, frame.FrameName);
                
                var processStartInfo = this.CreateProcessStartInfo(inputFile, outputFile);
                this._logger.LogInformation($"{processStartInfo.FileName} {processStartInfo.Arguments}");
                do
                {
                    var process = new Process {StartInfo = processStartInfo};
                    process.ErrorDataReceived +=
                        (sender, args) => this._logger.LogError($"Waifu2x Vulkan output: {args.Data}");
                    process.OutputDataReceived +=
                        (sender, args) => this._logger.LogDebug($"Waifu2x Vulkan output: {args.Data}");

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
                } while (!await this._fileProxy.ExistsAsync(outputFile));

        }

        public Task<bool> IsAlreadyUpscaled(Frame frame)
        {
            return this._fileProxy.ExistsAsync(this.CreateScaledFrameFullName(frame));
        }

        public string GetScaledPath()
        {
            return this._outputPath;
        }

        private ProcessStartInfo CreateProcessStartInfo(string inputFile, string outputFile)
        {
            var processStartInfo = new ProcessStartInfo("bash")
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = this._waifu2XSettings.WorkingDir,
                Arguments = $"-c \"{this._waifu2XSettings.Executable} "
            };

            this.SetArgumentsString(processStartInfo, this._waifu2XSettings);
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
            return Path.Combine(this._outputPath, frame.FrameName);
        }
    }
}