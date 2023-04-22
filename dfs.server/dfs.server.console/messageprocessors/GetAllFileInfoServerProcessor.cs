using dfs.core.common.dispatcher;
using dfs.core.common.models;
using dfs.server.console;

public class GetAllFileInfoServerProcessor : IMessageProcessor
{
    private FollowUpMessage _followUpMessage { get; set; } = new FollowUpMessage();
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.GET_ALL_FILEINFO, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(DocumentServerSession);
    }

    public FollowUpMessage FollowUpMessage()
    {
        return _followUpMessage;
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        _followUpMessage = new FollowUpMessage();
        var documents = ServerStorage.GetDocuments();
        var result = new List<Document>();
        foreach (var document in documents)
        {
            if (CacheStorage.Exists(document.Name))
                result.Add(document.Copy(document.Cost / 2));
            else
                result.Add(document.Copy());
        }
        var getAllFileInfoMessage = new GetAllFileInfoMessage
        {
            Documents = result
        };
        _followUpMessage.FollowUpText = baseMessage.Reply(getAllFileInfoMessage.AsJson()).AsJson();
        return ProcessMessageStatus.Processed;
    }

    public ProcessMessageStatus ProcessMessage(byte[] buffer)
    {
        return ProcessMessageStatus.Processed;
    }
}