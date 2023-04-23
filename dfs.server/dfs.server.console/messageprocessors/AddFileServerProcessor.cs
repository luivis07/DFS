using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.helpers;
using dfs.core.common.models;

namespace dfs.server.console.messageprocessors;

public class AddFileServerProcessor : IMessageProcessor
{
    private byte[] _contents { get; set; } = new byte[0];
    private Document _currentDocument = new Document();
    private FollowUpMessage _followUpMessage { get; set; } = new FollowUpMessage();
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.ADMIN_ADD_DOCUMENT, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(DocumentServerSession);
    }

    public FollowUpMessage FollowUpMessage()
    {
        return _followUpMessage;
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        _followUpMessage = new FollowUpMessage();
        var document = JsonSerializer.Deserialize<Document>(baseMessage.Payload);
        if (document != null)
        {
            Console.WriteLine($"({baseMessage.SessionId}): received {document.NameWithExtension}");
            var directory = PathProvider.GetDatastoreBasePath();
            Directory.CreateDirectory(directory);
            var fileName = Path.Combine(directory, document.NameWithExtension ?? "temp.txt");
            _currentDocument = document;
            _currentDocument.FullPath = fileName;
            return ProcessMessageStatus.Processing;
        }
        return ProcessMessageStatus.Error;
    }

    public ProcessMessageStatus ProcessMessage(byte[] buffer)
    {
        _contents = Combine(_contents, buffer);
        if (_contents.Length == _currentDocument.Size)
        {
            File.WriteAllBytes(_currentDocument.FullPath, _contents.ToArray());
            _contents = new byte[0];
            _currentDocument = new Document();
            return ProcessMessageStatus.Processed;
        }
        return ProcessMessageStatus.Processing;
        
    }
    public static byte[] Combine(byte[] first, byte[] second)
    {
        byte[] bytes = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
        Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
        return bytes;
    }
}