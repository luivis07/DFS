using System.Text.Json;
using dfs.cache.console;
using dfs.core.common.dispatcher;
using dfs.core.common.models;
using dfs.datastore.console;

namespace dfs.server.console.messageprocessors;

public class AdminRemoveDocument : IMessageProcessor
{
    private FollowUpMessage _followUpMessage { get; set; } = new FollowUpMessage();
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.ADMIN_REMOVE_DOCUMENT, StringComparison.OrdinalIgnoreCase) &&
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
        if (message != null && message.Document != null)
        {
            var result = ServerStorage.RemoveDocument(message.Document.Name);
            CacheStorage.RemoveDocument(message.Document.Name);
            if (result)
                return ProcessMessageStatus.Processed;
            else
                return ProcessMessageStatus.Error;
        }
        return ProcessMessageStatus.Error;
    }

    public ProcessMessageStatus ProcessMessage(byte[] buffer)
    {
        return ProcessMessageStatus.Processed;
    }
}