namespace UpscaleVulkan.Application.States
{
    using System.Threading.Tasks;
    using Core;
    using Services;

    public class UpscaleState : State
    {
        private readonly IWaifu2x waifu2X;
        private readonly FinalizeVideo nextState;

        public UpscaleState(IWaifu2x waifu2X, FinalizeVideo nextState)
        {
            this.waifu2X = waifu2X;
            this.nextState = nextState;
        }

        public override async Task Execute()
        {
            foreach (Frame frame in this.UpscaleContext.Video.GetFrames())
            {
                if (frame.IsAlreadyScaled)
                {
                    continue;
                }

                await this.waifu2X.Upscale(frame);
                frame.SetToUpscaled();
            }
            
            this.UpscaleContext.SetState(this.nextState);
        }
    }
}