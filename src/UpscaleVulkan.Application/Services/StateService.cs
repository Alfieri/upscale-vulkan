namespace UpscaleVulkan.Application.Services
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.MediaInfo;
    using Core.Settings;
    using Helpers;    
    using States;

    public class StateService : IStateService
    {
        private readonly ScalingDone scalingDoneState;
        private readonly FinalizeVideo finalizeVideoState;
        private readonly UpscaleState upscaleState;
        private readonly ExtractFramesState extractFramesState;


        public StateService(IVideoConverter videoConverter, IWaifu2x waifu2X)
        {
            this.scalingDoneState = new ScalingDone();
            this.finalizeVideoState = new FinalizeVideo(videoConverter, this.scalingDoneState);
            this.upscaleState = new UpscaleState(waifu2X, this.finalizeVideoState);
            this.extractFramesState = new ExtractFramesState(videoConverter, this.upscaleState);
        }

        public Task<State> GetCurrentState(UpscaleSettings upscaleSettings, FfprobeJson videoInfo)
        {
            var expectedNumberOfFrames = int.Parse(videoInfo.Streams.VideoStream().NumberOfFrames ?? "0");
            if (Directory.EnumerateFiles(upscaleSettings.FullFramesPath).Count() < expectedNumberOfFrames)
            {
                return Task.FromResult((State)this.extractFramesState);
            }

            if (Directory.EnumerateFiles(upscaleSettings.FullScaledPath).Count() < Directory.EnumerateFiles(upscaleSettings.FullFramesPath).Count())
            {
                return Task.FromResult((State)this.upscaleState);
            }

            return Task.FromResult((State)this.finalizeVideoState);
        }
    }
}