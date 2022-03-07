namespace UpscaleVulkan.Core;

public class Frame
{
    public Frame(string framePath, string frameName, bool isAlreadyScaled = false)
    {
        this.FrameName = frameName;
        this.FramePath = framePath;
        this.IsAlreadyScaled = isAlreadyScaled;
    }

    public string FramePath { get; }

    public string FrameName { get; }

    public bool IsAlreadyScaled { get; private set; }

    public void SetToUpscaled()
    {
        this.IsAlreadyScaled = true;
    }

    public override string ToString()
    {
        return this.FrameName;
    }
}