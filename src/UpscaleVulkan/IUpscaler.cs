namespace UpscaleVulkan
{
    using System.Threading.Tasks;

    public interface IUpscaler
    {
        Task Upscale(IWaifu2x waifu2xImplementation);
        
        Task ExtractFrames(IVideoConverter videoConverter);
        
        Task CreateVideoFromScaledFrames(IVideoConverter videoConverter);
        
        Task CreateFinaleVideo(IVideoConverter videoConverter);
    }
}