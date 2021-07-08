namespace UpscaleVulkan.Core.Settings
{
    public class UpscaleSettings
    {
        public string VideoFile { get; set; } = string.Empty;

        public string TempPath { get; set; } = string.Empty;

        public string ScaledPath { get; set; } = string.Empty;

        public bool Resume { get; set; }
    }
}