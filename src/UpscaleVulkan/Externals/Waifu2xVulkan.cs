namespace UpscaleVulkan.Externals
{
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Dtos;

    public class Waifu2xVulkan : IWaifu2x
    {
        private string _outputPath;

        public Waifu2xVulkan(Waifu2xSettings waifu2XSettings)
        {
            this._outputPath = waifu2XSettings.OutputPath;
            this.ProcessStartInfo = new ProcessStartInfo(waifu2XSettings.Executable)
            {
                Arguments = this.BuildArguments(waifu2XSettings)
            };
        }

        internal ProcessStartInfo ProcessStartInfo { get; }

        public async Task<ScaledFrame> Upscale(Frame frame)
        {
            string outFile = Path.Combine(this._outputPath, frame.FrameName);
            this.ProcessStartInfo.Arguments +=
                $"-i {Path.Combine(frame.FramePath, frame.FrameName)} -o {outFile}";
            var process = Process.Start(this.ProcessStartInfo);
            process.WaitForExit();
            bool scaledFrameExists = await Task.Run<bool>(() => File.Exists(outFile));
            if (scaledFrameExists)
            {
                return new ScaledFrame(frame);
            }

            // TODO: handle error
            return null;
        }

        private string BuildArguments(Waifu2xSettings waifu2XSettings)
        {
            var arguments = new StringBuilder();
            if (!string.IsNullOrEmpty(waifu2XSettings.ModelPath))
            {
                arguments.Append($"-m {waifu2XSettings.ModelPath} ");
            }

            if (waifu2XSettings.Scale > 0)
            {
                arguments.Append($"-s {waifu2XSettings.Scale} ");
            }

            if (waifu2XSettings.NoiseLevel > 0)
            {
                arguments.Append($"-n {waifu2XSettings.NoiseLevel} ");
            }

            return arguments.ToString();
        }
    }
}