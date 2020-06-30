namespace UpscaleVulkan
{
    using System.Threading.Tasks;
    using UpscaleVulkan.Model;

    public class Upscaler : IUpscaler
    {
        private readonly UpscaleSettings _upscaleSettings;

        public Upscaler(UpscaleSettings upscaleSettings)
        {
            this._upscaleSettings = upscaleSettings;
        }

        public Task Upscale(Video video)
        {
            throw new System.NotImplementedException();
        }
    }
}