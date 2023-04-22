using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.models;

namespace dfs.client.console.messageprocessors;

public class SimpleMessageProcessor : IMessageProcessor
{
    private FollowUpMessage _followUpMessage { get; set; } = new FollowUpMessage();
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.SIMPLE, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(DocumentClient);
    }

    public FollowUpMessage FollowUpMessage()
    {
        return _followUpMessage;
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        _followUpMessage = new FollowUpMessage();
        if (string.IsNullOrWhiteSpace(baseMessage.Payload))
            return ProcessMessageStatus.Error;
        var message = JsonSerializer.Deserialize<SimpleMessage>(baseMessage.Payload);
        if (message != null)
        {
            Console.WriteLine(message.TextMessage);
            return ProcessMessageStatus.Reset;
        }
        return ProcessMessageStatus.Error;
    }

    public ProcessMessageStatus ProcessMessage(byte[] buffer)
    {
        return ProcessMessageStatus.Processed;
    }
}