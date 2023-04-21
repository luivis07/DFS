using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.models;

namespace dfs.server.console.messageprocessors;

public class GetFileServerProcessor : IMessageProcessor
{
    private string? _followUpMessage { get; set; }
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.GET_FILE, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(DocumentServerSession);
    }

    public string? FollowUpMessage()
    {
        return _followUpMessage;
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        var message = JsonSerializer.Deserialize<GetFileMessage>(baseMessage.Payload);
        if (message != null)
        {
            Console.WriteLine($"({baseMessage.SessionId}): received request for {message.Document?.Name}");
            var contents = ServerStorage.GetDocumentContent(message.Document?.FullPath);
            var reply = message.Reply(contents);
            _followUpMessage = baseMessage.Reply(reply.AsJson()).AsJson();
            Console.WriteLine($"({baseMessage.SessionId}): sent {contents.Length} bytes");
            return ProcessMessageStatus.Processed;
        }
        return ProcessMessageStatus.Error;
    }
}