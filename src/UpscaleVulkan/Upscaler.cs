namespace UpscaleVulkan
{
    using System.Threading.Tasks;
    using Core;
    using Dtos;

    public class Upscaler : IUpscaler
    {
        private readonly UpscaleSettings _upscaleSettings;

        public Upscaler(UpscaleSettings upscaleSettings)
        {
            this._upscaleSettings = upscaleSettings;
        }

        public Task Upscale(Video videoFile, IWaifu2x waifu2xImplementation)
        {
            return videoFile.Upscale(waifu2xImplementation);
        }
    }
}