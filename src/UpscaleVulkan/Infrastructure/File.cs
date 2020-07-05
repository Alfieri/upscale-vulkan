namespace UpscaleVulkan.Infrastructure
{
    using System.Threading.Tasks;

    public class File : IFileProxy
    {
        public Task<bool> ExistsAsync(string file)
        {
            return Task.Run(() => System.IO.File.Exists(file));
        }
    }
}