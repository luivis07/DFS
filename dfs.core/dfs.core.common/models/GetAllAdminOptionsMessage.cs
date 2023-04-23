using System.Text.Json;

namespace dfs.core.common.models;

public class GetAllAdminOptionsMessage
{
    public string Menu { get; set; } = string.Empty;
    public string AsJson()
    {
        return JsonSerializer.Serialize(this);
    }
}