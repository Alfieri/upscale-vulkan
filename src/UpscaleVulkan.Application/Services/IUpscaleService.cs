namespace UpscaleVulkan.Application.Services
{
    using System.Threading.Tasks;
    using Core;

    public interface IUpscaleService
    {
        Task Upscale(Video video);
    }
}