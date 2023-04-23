using dfs.core.common.dispatcher;
using dfs.core.common.models;

namespace dfs.client.console.messageprocessors;

public class AdminMessageProvider
{
    private IEnumerable<IMessageProcessor> _messageProcessors { get; set; }
    public AdminMessageProvider()
    {
        _messageProcessors = new List<IMessageProcessor>
        {
            new GetFilesForRemovalProcessor()
        };
    }
    public IMessageProcessor? GetMessageProcessor(BaseMessage baseMessage, Type sender)
    {
        return _messageProcessors.FirstOrDefault(m => m.AppliesTo(baseMessage, sender));
    }
}