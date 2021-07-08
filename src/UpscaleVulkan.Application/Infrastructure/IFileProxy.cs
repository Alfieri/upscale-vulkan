namespace UpscaleVulkan.Application.Infrastructure
{
    using System.Threading.Tasks;

    public interface IFileProxy
    {
        Task<bool> ExistsAsync(string file);
    }
}