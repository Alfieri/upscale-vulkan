namespace UpscaleVulkan.Core.Settings
{
    public class FfmpegSettings
    {
        public string? FfmpegBin { get; set; } = "/usr/bin/ffmpeg";
        
        public string Framerate { get; set; } = "29.97";
        
        public string? HardwareAcceleration { get; set; } = "auto";

        public string? Codec { get; set; } = "h264_nvenc";

        public string? Preset { get; set; } = "slow";

        public string? AdditionalCodecParameter { get; set; } =
            "-profile:v high -tune:v hq -rc:v vbr -cq:v 19 -qmin 18 -qmax 24 -b:v 2500k -maxrate:v 5000k -bufsize:v 5000k -bf:v 4";

        public string? IntermediateVideoFile { get; set; } = "intermediate.mp4";

        public string? VideoToFramesPixFormat { get; set; } = "rgba64be";

        public string? FramesToVideoPixFormat { get; set; } = "yuv420p";

        public string? ConcatVideosParameter { get; set; } =
            "-map 0:v -map 1:a? -map 1:s? -map 1:d? -map 1:t? -c copy -map_metadata 0";
    }
}