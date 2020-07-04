namespace UpscaleVulkan.Core
{
    public class ScaledFrame
    {
        public ScaledFrame(string framePath, string frameName)
        {
            this.FramePath = framePath;
            this.FrameName = frameName;
        }
        
        public string FramePath { get; private set; }

        public string FrameName { get; private set; }
    }
}