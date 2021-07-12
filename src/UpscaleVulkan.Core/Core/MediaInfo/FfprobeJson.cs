namespace UpscaleVulkan.Core.MediaInfo
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class FfprobeJson
    {
        [JsonPropertyName("streams")]
        public List<StreamInfo> Streams { get; set; }

        [JsonPropertyName("format")]
        public FormatInfo Format { get; set; }
    }
}