namespace UpscaleVulkan.Core.MediaInfo
{
    using System.Text.Json.Serialization;

    public class FormatInfo
    {
        [JsonPropertyName("filename")]
        public string? Filename { get; set; }

        [JsonPropertyName("nb_streams")]
        public int NumberOfStreams { get; set; }

        [JsonPropertyName("format_name")]
        public string? FormatName { get; set; }
        
        [JsonPropertyName("format_name_long")]
        public string? FullFormatName { get; set; }

        [JsonPropertyName("duration")]
        public string? DurationInMilliseconds { get; set; }

        [JsonPropertyName("size")]
        public string? Size { get; set; }
        
        [JsonPropertyName("bit_rate")]
        public string? BitRate { get; set; }
    }
}