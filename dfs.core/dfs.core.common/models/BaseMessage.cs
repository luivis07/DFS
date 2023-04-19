using System.Text.Json;

namespace dfs.core.common.models;

public class BaseMessage
{
    public string MessageType { get; set; } = "Simple";
    public Guid SessionId { get; set; }
    public string Payload { get; set; } = string.Empty;

    public BaseMessage Reply(string payload)
    {
        return new BaseMessage
        {
            SessionId = this.SessionId,
            MessageType = this.MessageType,
            Payload = payload
        };
    }
    public string AsJson()
    {
        return JsonSerializer.Serialize(this);
    }
}