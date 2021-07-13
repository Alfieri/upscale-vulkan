namespace UpscaleVulkan.Web.Services
{
    using System.Threading.Tasks;
    using Core.MediaInfo;
    
    public interface IVideoInfoService
    {
        Task<FfprobeJson> GetVideoInfo();

        string GetHumanReadableDuration(FfprobeJson mediaInfo);
    }
}