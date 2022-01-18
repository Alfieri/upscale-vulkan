namespace UpscaleVulkan.Web.Services
{
    using System.Threading.Tasks;
    
    using Core;
    using Core.MediaInfo;
    
    public interface IUpscaleComponentViewService
    {
        FfprobeJson GetVideoInfo();
        
        Video GetVideo();

        Task InitializeAsync();
        
        Task ProcessVideo();
    }
}