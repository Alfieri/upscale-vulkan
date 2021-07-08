namespace UpscaleVulkan.Core
{
    using System.IO;

    public class IntermediateVideo
    {
        public IntermediateVideo(Video originalVideo, FileInfo intermediateVideoFile)
        {
            this.OriginalOriginalVideo = originalVideo;
            this.IntermediateVideoFile = intermediateVideoFile;
        }
        
        public FileInfo? IntermediateVideoFile { get; }

        public Video OriginalOriginalVideo { get; }
    }
}