using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.models;

namespace dfs.client.console.messageprocessors;

public class GetAllFileInfoClientProcessor : IMessageProcessor
{
    private string? _followUpMessage { get; set; }
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.GET_ALL_FILEINFO, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(DocumentClient);
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        var message = JsonSerializer.Deserialize<GetAllFileInfoMessage>(baseMessage.Payload);
        if (message != null)
        {
            var selection = ClientUI.DisplayDocuments(message.Documents);
            if (selection != null)
            {
                var fileMessage = new GetFileMessage
                {
                    Document = selection
                };
                Console.WriteLine($"({baseMessage.SessionId}): requested {selection.Name}");
                _followUpMessage = baseMessage.Reply(fileMessage.AsJson(), MessageType.GET_FILE).AsJson();
                Console.WriteLine($"({baseMessage.SessionId}): request sent");
                return ProcessMessageStatus.Processed;
            }
            else
            {
                return ProcessMessageStatus.Stop;
            }
        }
        return ProcessMessageStatus.Error;
    }

    public FollowUpMessage FollowUpMessage()
    {
        return new FollowUpMessage { FollowUpText = _followUpMessage };
    }

    public ProcessMessageStatus ProcessMessage(byte[] buffer)
    {
        return ProcessMessageStatus.Processed;
    }
}