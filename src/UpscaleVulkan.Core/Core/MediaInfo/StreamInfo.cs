namespace UpscaleVulkan.Core.MediaInfo;

using System.Text.Json.Serialization;

public class StreamInfo
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("codec_name")]
    public string? CodecName { get; set; }
        
    [JsonPropertyName("codec_long_name")]
    public string? FullCodecName { get; set; }
        
    [JsonPropertyName("profile")]
    public string? Profile { get; set; }
        
    [JsonPropertyName("codec_type")]
    public string? CodecType { get; set; }
        
    [JsonPropertyName("width")]
    public int Width { get; set; }
        
    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("pix_fmt")]
    public string? PixFormat { get; set; }

    [JsonPropertyName("r_frame_rate")]
    public string? Framerate { get; set; }

    [JsonPropertyName("bit_rate")]
    public string? BitRate { get; set; }

    [JsonPropertyName("nb_frames")]
    public string? NumberOfFrames { get; set; }
}