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
    public GetFileMessage Reply(byte[] contents)
    {
        return new GetFileMessage
        {
            Document = this.Document == null ? new Document() :
            new Document
            {
                NameWithExtension = this.Document.NameWithExtension,
                Cost = this.Document.Cost,
                QuantityAvailable = this.Document.QuantityAvailable,
                Size = this.Document.Size,
                FullPath = this.Document.FullPath
            },
            Contents = contents
        };
    }
}