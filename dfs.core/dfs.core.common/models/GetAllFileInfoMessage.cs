using System.Text.Json;

namespace dfs.core.common.models;

public class GetAllFileInfoMessage
{
    public IEnumerable<Document> Documents { get; set; } = Enumerable.Empty<Document>();

    public string AsJson()
    {
        return JsonSerializer.Serialize(this);
    }
}