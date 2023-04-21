using dfs.core.common.dispatcher;
using dfs.core.common.models;
using dfs.server.console;

public class GetAllFileInfoServerProcessor : IMessageProcessor
{
    private string? _followUpMessage { get; set; }
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.GET_ALL_FILEINFO, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(DocumentServerSession);
    }

    public string? FollowUpMessage()
    {
        return _followUpMessage;
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        var getAllFileInfoMessage = new GetAllFileInfoMessage
        {
            Documents = ServerStorage.GetDocuments()
        };
        _followUpMessage = baseMessage.Reply(getAllFileInfoMessage.AsJson()).AsJson();
        return ProcessMessageStatus.Processed;
    }
}