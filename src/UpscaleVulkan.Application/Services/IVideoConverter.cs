namespace UpscaleVulkan.Application.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Core;
    using Core.MediaInfo;

    public interface IVideoConverter
    {
        Task<List<Frame>> ExtractFrames(Video video);
        
        Task<FileInfo> CreateVideoFromFrames();
        
        Task CreateFinaleVideo(IntermediateVideo intermediateVideo);
        
        List<Frame> GetFrames();
        Task<FfprobeJson> GetVideoInfo(Video video);
    }
}