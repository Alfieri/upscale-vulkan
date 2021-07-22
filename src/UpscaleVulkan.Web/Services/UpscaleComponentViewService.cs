namespace UpscaleVulkan.Web.Services
{
    using System.Threading.Tasks;
    using UpscaleVulkan.Application.Services;
    using Application;
    using Core;
    using Core.MediaInfo;
    using Core.Settings;
    
    public class UpscaleComponentViewService : IUpscaleComponentViewService
    {
        private readonly IVideoConverter videoConverter;
        private readonly ISettingsService settingsService;
        private readonly IStateService stateService;
        private UpscaleSettings upscaleSettings;
        private Video video;
        private FfprobeJson videoInfo;
        private UpscaleContext context;

        public UpscaleComponentViewService(IVideoConverter videoConverter, ISettingsService settingsService, IStateService stateService)
        {
            this.videoConverter = videoConverter;
            this.settingsService = settingsService;
            this.stateService = stateService;
        }

        public FfprobeJson GetVideoInfo()
        {
            return this.videoInfo;
        }

        public async Task InitializeAsync()
        {
            this.upscaleSettings = await this.settingsService.LoadSettingsAsync<UpscaleSettings>();
            if (string.IsNullOrEmpty(this.upscaleSettings.VideoFile))
            {
                this.video = null;
                this.videoInfo = null;
            }
            else
            {
                this.video = new Video(this.upscaleSettings.VideoFile);
                this.videoInfo = await this.videoConverter.GetVideoInfo(this.video);
                var currentState = await this.stateService.GetCurrentState(this.upscaleSettings, this.videoInfo);
                this.context = new UpscaleContext(currentState) { Video = this.video };
            }
        }

        public async Task ProcessVideo()
        {
            await this.context.ProcessVideo();
        }

        public Video GetVideo()
        {
            return this.video;
        }
    }
}