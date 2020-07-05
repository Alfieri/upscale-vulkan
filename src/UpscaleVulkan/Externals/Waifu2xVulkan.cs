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
            this.ProcessStartInfo = new ProcessStartInfo(waifu2XSettings.Executable);
            this.SetArguments(waifu2XSettings);
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

        private void SetArguments(Waifu2xSettings waifu2XSettings)
        {
            if (!string.IsNullOrEmpty(waifu2XSettings.ModelPath))
            {
                this.ProcessStartInfo.ArgumentList.Add($"-m {waifu2XSettings.ModelPath}");
            }

            if (waifu2XSettings.Scale > 0)
            {
                this.ProcessStartInfo.ArgumentList.Add($"-s {waifu2XSettings.Scale}");
            }

            if (waifu2XSettings.NoiseLevel > 0)
            {
                this.ProcessStartInfo.ArgumentList.Add($"-n {waifu2XSettings.NoiseLevel}");
            }
        }
    }
}