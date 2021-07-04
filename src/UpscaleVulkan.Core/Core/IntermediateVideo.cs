namespace UpscaleVulkan.Core
{
    using System.IO;
    using System.Threading.Tasks;

    public class IntermediateVideo
    {
        private readonly Video _video;

        public IntermediateVideo(Video video)
        {
            this._video = video;
        }
        
        public FileInfo? IntermediateVideoFile { get; private set; }

        public Video OriginalVideo => this._video;
        
        public async Task CreateVideoFromUpscaledFrames(IVideoConverter videoConverter, string scaledPath)
        {
            this.IntermediateVideoFile = await videoConverter.CreateVideoFromFrames(scaledPath);
        }
        
        public Task CreateFinaleVideo(IVideoConverter videoConverter)
        {
            return videoConverter.CreateFinaleVideo(this);
        }
    }
}