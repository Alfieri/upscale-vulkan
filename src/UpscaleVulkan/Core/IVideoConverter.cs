namespace UpscaleVulkan
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;

    public interface IVideoConverter
    {
        Task<List<Frame>> ExtractFrames(Video video);
        
        Task<IntermediateVideo> CreateVideoFromFrames(double framerate, List<ScaledFrame> scaledFrames);
        
        Task CreateFinaleVideo(Video video);
    }
}