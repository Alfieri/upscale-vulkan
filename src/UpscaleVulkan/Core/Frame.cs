namespace UpscaleVulkan.Core
{
    using System.Threading.Tasks;

    public class Frame
    {
        public Frame(string framePath, string frameName, bool isUpscaled = false)
        {
            this.FrameName = frameName;
            this.FramePath = framePath;
            this.IsUpscaled = isUpscaled;
        }

        public string FramePath { get; private set; }

        public string FrameName { get; private set; }

        public bool IsUpscaled { get; private set; }

        public async Task<ScaledFrame> Upscale(IWaifu2x waifu2X)
        {
            return !this.IsUpscaled ? await waifu2X.Upscale(this) : new ScaledFrame(this);
        }
    }
}