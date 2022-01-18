namespace UpscaleVulkan.Application
{
    using System.Threading.Tasks;
    using Core;
    using States;

    public class UpscaleContext
    {
        private State currentState;

        public UpscaleContext(State currentState)
        {
            this.currentState = currentState;
        }

        public Video Video { get; init; }

        public async Task ProcessVideo()
        {
            while (this.currentState is not ScalingDone)
            {
                this.currentState.SetContext(this);
                await this.currentState.Execute();
            }
        }

        public void SetState(State nextState)
        {
            this.currentState = nextState;
        }
    }
}