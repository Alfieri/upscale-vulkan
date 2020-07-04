namespace UpscaleVulkan.Core
{
    public class ScaledFrame
    {
        public ScaledFrame(Frame frame)
        {
            this.FrameName = frame.FrameName;
            this.FramePath = frame.FramePath;
        }
        
        public string FramePath { get; private set; }

        public string FrameName { get; private set; }
    }
}