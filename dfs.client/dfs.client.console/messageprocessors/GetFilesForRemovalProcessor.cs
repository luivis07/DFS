using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.models;

namespace dfs.client.console.messageprocessors;

public class GetFilesForRemovalProcessor : IMessageProcessor
{
    private FollowUpMessage _followUpMessage { get; set; } = new FollowUpMessage();
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.GET_ALL_FILEINFO, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(AdminDocumentClient);
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        _followUpMessage = new FollowUpMessage();
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
                Console.WriteLine($"({baseMessage.SessionId}): remove {selection.Name}");
                _followUpMessage.FollowUpText = baseMessage.Reply(fileMessage.AsJson(), MessageType.ADMIN_REMOVE_DOCUMENT).AsJson();
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
        return _followUpMessage;
    }

    public ProcessMessageStatus ProcessMessage(byte[] buffer)
    {
        return ProcessMessageStatus.Processed;
    }
}