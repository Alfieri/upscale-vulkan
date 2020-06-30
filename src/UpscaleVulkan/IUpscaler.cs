namespace UpscaleVulkan
{
    using System.Threading.Tasks;
    using UpscaleVulkan.Model;

    public interface IUpscaler
    {
        Task Upscale(Video video);
    }
}