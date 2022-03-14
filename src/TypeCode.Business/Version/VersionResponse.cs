using System.Text.Json.Serialization;

namespace TypeCode.Business.Version;

internal class VersionResponse
{
    public VersionResponse()
    {
        TagName = string.Empty;
    }
    
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }
}