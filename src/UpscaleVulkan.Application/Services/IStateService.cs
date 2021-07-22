namespace UpscaleVulkan.Application.Services
{
    using System.Threading.Tasks;
    using Core.MediaInfo;
    using Core.Settings;
    using States;

    public interface IStateService
    {
        Task<State> GetCurrentState(UpscaleSettings upscaleSettings, FfprobeJson videoInfo);
    }
}