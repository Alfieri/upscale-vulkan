namespace UpscaleVulkan
{
    using System.Threading.Tasks;
    using Core;

    public interface IUpscaler
    {
        Task Upscale(Video videoFile, IWaifu2x waifu2xImplementation);
    }
}