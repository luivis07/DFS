using System.Text.Json;
using dfs.cache.console;
using dfs.core.common.dispatcher;
using dfs.core.common.helpers;
using dfs.core.common.models;
using dfs.datastore.console;

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
        if (message != null && message.Document != null)
        {
            Console.WriteLine($"({baseMessage.SessionId}): received request for {message.Document.Name}");
            if (ServerStorage.IsAvailable(message.Document.Name))
            {
                var isInCache = CacheStorage.Exists(message.Document.Name);
                var contents = isInCache ? CacheStorage.GetDocumentContent(message.Document.Name) :
                                             ServerStorage.GetDocumentContent(message.Document.FullPath).ToArray();
                _followUpMessage.FollowUpText = baseMessage.Reply(message.Reply().AsJson()).AsJson();
                _followUpMessage.FollowUpContent = contents;
                ServerStorage.DecreaseQuantity(message.Document.Name);
                Console.WriteLine($"({baseMessage.SessionId}): sending {contents.Length} bytes");
                if (!isInCache)
                {
                    var result = CacheStorage.Add(message.Document, contents);
                    if (result)
                        Console.WriteLine($"({baseMessage.SessionId}): added {contents.Length} to cache");
                    else
                        Console.WriteLine($"({baseMessage.SessionId}): unable to add {contents.Length} to cache");

                    Console.WriteLine($"({baseMessage.SessionId}): cache usage {CacheStorage.UsedSpace}/{CacheStorage.TotalSpace}");
                }
            }
            else
            {
                var returnMessage = $"({baseMessage.SessionId}): {message.Document.Name} is not available";
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