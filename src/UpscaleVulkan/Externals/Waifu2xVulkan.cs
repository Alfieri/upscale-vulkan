namespace UpscaleVulkan.Externals
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Core;
    using Dtos;
    using Exceptions;
    using Infrastructure;
    using Microsoft.Extensions.Logging;

    public class Waifu2xVulkan : IWaifu2x
    {
        private readonly Waifu2xSettings _waifu2XSettings;
        
        private readonly IFileProxy _fileProxy;
        
        private readonly ILogger<Waifu2xVulkan> _logger;
        
        private string _outputPath;

        public Waifu2xVulkan(Waifu2xSettings waifu2XSettings, IFileProxy fileProxy, ILogger<Waifu2xVulkan> logger)
        {
            this._waifu2XSettings = waifu2XSettings;
            this._fileProxy = fileProxy;
            this._logger = logger;
            if (string.IsNullOrEmpty(waifu2XSettings.OutputPath))
            {
                throw new ArgumentNullException(nameof(waifu2XSettings.OutputPath));
            }
            
            this._outputPath = waifu2XSettings.OutputPath;
        }

        public async Task<ScaledFrame> Upscale(Frame frame)
        {
            string outputFile = Path.Combine(this._outputPath, frame.FrameName);
            string inputFile = Path.Combine(frame.FramePath, frame.FrameName);

            bool scaledFrameExists = await this._fileProxy.ExistsAsync(outputFile);
            if (!scaledFrameExists)
            {
                var processStartInfo = this.CreateProcessStartInfo(inputFile, outputFile);
                this._logger.LogInformation($"{processStartInfo.FileName} {processStartInfo.Arguments}");
                var process = Process.Start(processStartInfo);
                process?.WaitForExit();
            }
            
            scaledFrameExists = await this._fileProxy.ExistsAsync(outputFile);
            if (scaledFrameExists)
            {
                return new ScaledFrame(frame);
            }

            throw new ScalingFailedException("Upscaled frame could not be found.");
        }

        private ProcessStartInfo CreateProcessStartInfo(string inputFile, string outputFile)
        {
            var processStartInfo = new ProcessStartInfo("bash")
            {
                // RedirectStandardError = true,
                // RedirectStandardOutput = true,
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
    }
}