using System.Text.Json;

namespace dfs.core.common.models;

public class BaseMessage
{
    public string MessageType { get; set; } = "Simple";
    public Guid SessionId { get; set; }
    public string Payload { get; set; } = string.Empty;

    public BaseMessage Reply(string payload, string messageType = "")
    {
        return new BaseMessage
        {
            SessionId = this.SessionId,
            MessageType = string.IsNullOrWhiteSpace(messageType) ? this.MessageType : messageType,
            Payload = payload
        };
    }
    public string AsJson()
    {
        return JsonSerializer.Serialize(this);
    }
}