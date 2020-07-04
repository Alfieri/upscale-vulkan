namespace UpscaleVulkan
{
    using System.Threading.Tasks;
    using Dtos;

    public class Upscaler : IUpscaler
    {
        private readonly UpscaleSettings _upscaleSettings;

        public Upscaler(UpscaleSettings upscaleSettings)
        {
            this._upscaleSettings = upscaleSettings;
        }

        public Task Upscale()
        {
            throw new System.NotImplementedException();
        }
    }
}