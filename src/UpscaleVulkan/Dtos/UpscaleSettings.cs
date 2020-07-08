namespace UpscaleVulkan.Dtos
{
    using System.IO;

    public class UpscaleSettings
    {
        public FileInfo VideoFile { get; private set; }

        public string TempPath { get; private set; }
    }
}