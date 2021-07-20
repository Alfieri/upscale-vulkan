namespace UpscaleVulkan.Application.States
{
    using System.Threading.Tasks;
    using Services;

    public class ExtractFramesState : State
    {
        private readonly IVideoConverter videoConverter;
        private readonly UpscaleState nextState;

        public ExtractFramesState(IVideoConverter videoConverter, UpscaleState nextState)
        {
            this.videoConverter = videoConverter;
            this.nextState = nextState;
        }

        public override async Task Execute()
        {
            var extractFrames = await this.videoConverter.ExtractFrames(this.UpscaleContext.Video);
            this.UpscaleContext.Video.AddFrames(extractFrames);
            this.UpscaleContext.SetState(this.nextState);
        }
    }
}