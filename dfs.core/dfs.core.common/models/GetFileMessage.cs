using System.Text.Json;

namespace dfs.core.common.models;

public class GetFileMessage
{
    public Document? Document { get; set; }
    public byte[]? Contents { get; set; }
    public string AsJson()
    {
        return JsonSerializer.Serialize(this);
    }
}