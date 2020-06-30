using System.Collections.Generic;
using System.IO;

namespace UpscaleVulkan.Model
{
    public class Video
    {
        private readonly FileInfo _videoFile;

        private int _framerate;

        private List<Frame> _frames;

        private List<ScaledFrame> _scaledFrames;

        public Video(UpscaleSettings upscaleSettings)
        {
            this._videoFile = upscaleSettings.VideoFile;
        }
    }
}