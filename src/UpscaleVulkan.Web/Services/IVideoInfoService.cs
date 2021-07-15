namespace UpscaleVulkan.Web.Services
{
    using System.Threading.Tasks;
    using Core;
    using Core.MediaInfo;
    
    public interface IVideoInfoService
    {
        Task<FfprobeJson> GetVideoInfo(Video video);

        string GetHumanReadableDuration(FfprobeJson mediaInfo);
    }
}