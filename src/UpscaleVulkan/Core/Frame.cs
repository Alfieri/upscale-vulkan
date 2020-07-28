namespace UpscaleVulkan.Core
{
    public class Frame
    {
        public Frame(string framePath, string frameName, bool isUpscaled = false)
        {
            this.FrameName = frameName;
            this.FramePath = framePath;
            this.IsUpscaled = isUpscaled;
        }

        public string FramePath { get; }

        public string FrameName { get; }

        public bool IsUpscaled { get; private set; }

        public void SetToUpscaled()
        {
            this.IsUpscaled = true;
        }

        public override string ToString()
        {
            return this.FrameName;
        }
    }
}