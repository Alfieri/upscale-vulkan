namespace UpscaleVulkan.Dtos
{
    public class FfmpegParameter
    {
        public string FfmpegBin { get; set; }
        
        public string TempPath { get; set; }

        public string HardwareAcceleration { get; set; }

        public string Codec { get; set; }

        public string Preset { get; set; }

        public string AdditionalCodecParameter { get; set; }
        
        public string IntermediateVideoFile { get; set; }

        public string VideoToFramesPixFormat { get; set; }

        public string FramesToVideoPixFormat { get; set; }
        
        public string ConcatVideosParameter { get; set; }
    }
}