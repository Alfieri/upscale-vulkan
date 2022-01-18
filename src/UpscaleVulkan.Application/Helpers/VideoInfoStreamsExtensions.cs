namespace UpscaleVulkan.Application.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.MediaInfo;
    
    public static class VideoInfoStreamsExtensions
    {
        public static StreamInfo VideoStream(this List<StreamInfo> streams)
        {
            return streams.First(s => s.CodecType?.Equals(CodecType.Video) ?? false);
        }
    }
}