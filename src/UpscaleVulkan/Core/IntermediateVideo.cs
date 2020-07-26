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
        
        public async Task CreateVideoFromUpscaledFrames(IVideoConverter videoConverter)
        {
            if (this._video.ScaledFrames.Count <= 0)
            {
                return;
            }
            
            string scaledPath = this._video.ScaledFrames[0].FramePath; 
            this.IntermediateVideoFile = await videoConverter.CreateVideoFromFrames(this._video.Framerate, scaledPath);
        }
        
        public Task CreateFinaleVideo(IVideoConverter videoConverter)
        {
            return videoConverter.CreateFinaleVideo(this);
        }
    }
}