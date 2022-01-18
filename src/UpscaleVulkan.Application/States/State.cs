namespace UpscaleVulkan.Application.States
{
    using System.Threading.Tasks;

    public abstract class State
    {
        protected UpscaleContext UpscaleContext { get; private set; } = null!;

        public abstract Task Execute();

        public void SetContext(UpscaleContext upscaleContext)
        {
            this.UpscaleContext = upscaleContext;
        }
    }
}