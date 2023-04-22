using System.Text.Json;

namespace dfs.core.common.models;

public class SimpleMessage
{
    public string TextMessage { get; set; } = string.Empty;

    public string AsJson()
    {
        return JsonSerializer.Serialize(this);
    }
}