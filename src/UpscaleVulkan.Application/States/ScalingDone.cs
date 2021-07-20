namespace UpscaleVulkan.Application.States
{
    using System.Threading.Tasks;

    public class ScalingDone : State
    {
        public override Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}