namespace UpscaleVulkan.Application.States
{
    using System.Threading.Tasks;
    using Core;
    using Services;

    public class FinalizeVideo : State
    {
        private readonly IVideoConverter videoConverter;
        private readonly ScalingDone nextState;

        public FinalizeVideo(IVideoConverter videoConverter, ScalingDone nextState)
        {
            this.videoConverter = videoConverter;
            this.nextState = nextState;
        }

        public override async Task Execute()
        {
            var upscaledVideo = await this.videoConverter.CreateVideoFromFrames();
            var intermediateVideo = new IntermediateVideo(this.UpscaleContext.Video, upscaledVideo);
            await this.videoConverter.CreateFinaleVideo(intermediateVideo);
            this.UpscaleContext.SetState(this.nextState);
        }
    }
}