namespace UpscaleVulkan.Dtos
{
    public class Waifu2xSettings
    {
        public string? ModelPath { get; set; }
        
        public string? WorkingDir { get; set; }

        public int Scale { get; set; }

        public int NoiseLevel { get; set; }
        
        public string? Executable { get; set; }
        
        public string?  OutputPath { get; set; }
    }
}