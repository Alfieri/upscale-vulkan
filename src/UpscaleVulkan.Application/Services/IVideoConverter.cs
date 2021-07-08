namespace UpscaleVulkan.Application.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Core;

    public interface IVideoConverter
    {
        Task<List<Frame>> ExtractFrames(Video video);
        
        Task<FileInfo> CreateVideoFromFrames();
        
        Task CreateFinaleVideo(IntermediateVideo intermediateVideo);
        
        List<Frame> GetFrames();
    }
}