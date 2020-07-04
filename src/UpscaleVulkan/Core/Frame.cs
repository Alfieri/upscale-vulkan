namespace UpscaleVulkan.Core
{
    using System.IO;
    using System.Threading.Tasks;

    public class Frame
    {
        public Frame(FileInfo frame, bool isUpscaled)
        {
            this.FrameName = frame.Name;
            this.FramePath = frame.DirectoryName;
            this.IsUpscaled = isUpscaled;
        }

        public string FramePath { get; private set; }

        public string FrameName { get; private set; }

        public bool IsUpscaled { get; private set; }

        public async Task<ScaledFrame> Upscale(IWaifu2x waifu2X)
        {
            return await waifu2X.Upscale(this);
        }
    }
}