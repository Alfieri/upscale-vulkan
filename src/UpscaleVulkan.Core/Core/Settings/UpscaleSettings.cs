namespace UpscaleVulkan.Core.Settings;

public class UpscaleSettings
{
    public string VideoFile { get; set; } = string.Empty;

    public string TempPath { get; set; } = string.Empty;

    public string ScaledPath { get; set; } = string.Empty;

    public string FramesPath { get; set; } = string.Empty;

    public string FullFramesPath => Path.Combine(this.TempPath, this.FramesPath);
        
    public string FullScaledPath => Path.Combine(this.TempPath, this.ScaledPath);
}