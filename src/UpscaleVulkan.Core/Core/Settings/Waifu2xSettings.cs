namespace UpscaleVulkan.Core.Settings
{
    public class Waifu2xSettings
    {
        public string? ModelPath { get; set; }
        
        public string? WorkingDir { get; set; }

        public int Scale { get; set; } = 2;

        public int NoiseLevel { get; set; } = 2;

        public string? Executable { get; set; } = "waifu2x-ncnn-vulkan";
        
        public string?  OutputPath { get; set; }
    }
}