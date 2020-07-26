namespace UpscaleVulkan.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IVideoConverter
    {
        Task<List<Frame>> ExtractFrames(Video video);
        
        Task<FileInfo> CreateVideoFromFrames(double framerate, string scaledInputPath);
        
        Task CreateFinaleVideo(IntermediateVideo intermediateVideo);
    }
}