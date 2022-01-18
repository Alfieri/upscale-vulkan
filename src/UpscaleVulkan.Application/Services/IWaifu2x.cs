namespace UpscaleVulkan.Application.Services
{
    using System.Threading.Tasks;
    using Core;

    public interface IWaifu2x
    {
        Task Upscale(Frame frame);

        Task<bool> IsAlreadyUpscaled(Frame frame);
        
        string GetScaledPath();
    }
}