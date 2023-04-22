using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.helpers;
using dfs.core.common.models;

namespace dfs.server.console.messageprocessors;

public class GetFileServerProcessor : IMessageProcessor
{
    private FollowUpMessage _followUpMessage { get; set; } = new FollowUpMessage();
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.GET_FILE, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(DocumentServerSession);
    }

    public FollowUpMessage FollowUpMessage()
    {
        return _followUpMessage;
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        _followUpMessage = new FollowUpMessage();
        var message = JsonSerializer.Deserialize<GetFileMessage>(baseMessage.Payload);
        if (message != null)
        {
            Console.WriteLine($"({baseMessage.SessionId}): received request for {message.Document?.Name}");
            if (ServerStorage.IsAvailable(message.Document?.Name ?? ""))
            {
                var contents = ServerStorage.GetDocumentContent(message.Document?.FullPath).ToArray();
                _followUpMessage.FollowUpText = baseMessage.Reply(message.Reply().AsJson()).AsJson();
                _followUpMessage.FollowUpContent = contents;
                Console.WriteLine($"({baseMessage.SessionId}): sending {contents.Length} bytes");
            }
            else
            {
                var returnMessage = $"({baseMessage.SessionId}): {message.Document?.Name} is not available";
                var simpleMessage = new SimpleMessage
                {
                    TextMessage = returnMessage
                };
                _followUpMessage.FollowUpText = baseMessage.Reply(simpleMessage.AsJson(), MessageType.SIMPLE).AsJson();
                Console.WriteLine(returnMessage);
            }
            return ProcessMessageStatus.Processed;
        }
        return ProcessMessageStatus.Error;
    }

    public ProcessMessageStatus ProcessMessage(byte[] buffer)
    {
        return ProcessMessageStatus.Processed;
    }
}