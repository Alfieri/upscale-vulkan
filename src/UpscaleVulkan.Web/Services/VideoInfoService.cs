namespace UpscaleVulkan.Web.Services
{
    using System.Threading.Tasks;
    using Core.MediaInfo;

    public class VideoInfoService : IVideoInfoService
    {
        public Task<FfprobeJson> GetVideoInfo()
        {
            throw new System.NotImplementedException();
        }

        public string GetHumanReadableDuration(FfprobeJson mediaInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}