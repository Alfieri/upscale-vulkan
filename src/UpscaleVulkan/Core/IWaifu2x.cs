namespace UpscaleVulkan
{
    using System.Threading.Tasks;
    using Core;

    public interface IWaifu2x
    {
        Task<ScaledFrame> Upscale(Frame frame);
    }
}