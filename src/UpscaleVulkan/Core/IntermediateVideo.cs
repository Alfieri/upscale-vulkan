namespace UpscaleVulkan.Core
{
    using System.IO;

    public class IntermediateVideo
    {
        public IntermediateVideo(FileInfo intermediateVideoFile)
        {
            this.IntermediateVideoFile = intermediateVideoFile;
        }
        
        public FileInfo IntermediateVideoFile { get; }
    }
}