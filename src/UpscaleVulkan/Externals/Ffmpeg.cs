namespace UpscaleVulkan.Externals
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Dtos;

    public class Ffmpeg : IVideoConverter
    {
        private readonly FfmpegParameter _ffmpegParameter;

        public Ffmpeg(FfmpegParameter ffmpegParameter)
        {
            this._ffmpegParameter = ffmpegParameter;
        }

        public Task<List<Frame>> ExtractFrames()
        {
            throw new System.NotImplementedException();
        }

        public Task<IntermediateVideo> CreateVideoFromFrames(double framerate, List<ScaledFrame> scaledFrames)
        {
            throw new System.NotImplementedException();
        }
    }
}