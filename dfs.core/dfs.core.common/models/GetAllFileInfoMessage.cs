using System.Text.Json;

namespace dfs.core.common.models;

public class GetAllFileInfoMessage : IAdminMessage
{
    public IEnumerable<Document> Documents { get; set; } = Enumerable.Empty<Document>();
    public Document? Document { get; set; }

    public string AsJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public byte[]? GetFollowUpContent()
    {
        return null;
    }

    public string GetMessageType()
    {
        return MessageType.GET_ALL_FILEINFO;
    }
}