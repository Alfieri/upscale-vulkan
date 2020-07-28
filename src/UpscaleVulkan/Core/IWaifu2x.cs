namespace UpscaleVulkan.Core
{
    using System.Threading.Tasks;

    public interface IWaifu2x
    {
        Task<ScaledFrame> Upscale(Frame frame);

        Task<bool> IsAlreadyUpscaled(Frame frame);
    }
}