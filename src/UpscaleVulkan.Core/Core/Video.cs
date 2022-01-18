namespace UpscaleVulkan.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Video
    {
        private readonly List<Frame> frames = new();

        public Video(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "Video file must be set");
            }
            
            this.VideoFile = new FileInfo(path);
        }
        
        public FileInfo VideoFile { get; }

        public void AddFrames(List<Frame> extractFrames)
        {
            this.frames.AddRange(extractFrames);
        }

        public List<Frame> GetFrames()
        {
            return this.frames;
        }
    }
}