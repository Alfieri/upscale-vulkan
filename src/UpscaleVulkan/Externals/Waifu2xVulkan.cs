namespace UpscaleVulkan.Externals
{
    using System.Threading.Tasks;
    using Core;
    using Dtos;

    public class Waifu2xVulkan : IWaifu2x
    {
        private readonly Waifu2xSettings _waifu2XSettings;

        public Waifu2xVulkan(Waifu2xSettings waifu2XSettings)
        {
            this._waifu2XSettings = waifu2XSettings;
        }

        public Task<ScaledFrame> Upscale(Frame frame)
        {
            throw new System.NotImplementedException();
        }
    }
}