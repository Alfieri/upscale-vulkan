using System.IO;

namespace UpscaleVulkan
{
    public class UpscaleSettings
    {
        public FileInfo VideoFile { get; private set; }

        public string TempPath { get; private set; }
    }
}