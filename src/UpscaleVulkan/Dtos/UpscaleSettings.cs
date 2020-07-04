namespace UpscaleVulkan.Dtos
{
    using System.IO;

    public class UpscaleSettings
    {
        public FileInfo VideoFile { get; private set; }

        public string TempPath { get; private set; }

        public FfmpegParameter FfmpegParameter { get; private set; }
        
        public Waifu2xSettings Waifu2XSettings { get; private set; }
    }
}