using System.Text.Json;
using dfs.core.common.dispatcher;
using dfs.core.common.helpers;
using dfs.core.common.models;
using dfs.core.common.settings;

namespace dfs.client.console.messageprocessors;

public class GetFileClientProcessor : IMessageProcessor
{
    private readonly ClientSettings _clientSettings;
    private byte[] _contents { get; set; } = new byte[0];
    private Document _currentDocument = new Document();
    public GetFileClientProcessor()
    {
        _clientSettings = SettingsReader.GetSettings().Client;
    }
    private string? _followUpMessage { get; set; }
    public bool AppliesTo(BaseMessage baseMessage, Type sender)
    {
        return string.Equals(baseMessage.MessageType, MessageType.GET_FILE, StringComparison.OrdinalIgnoreCase) &&
            sender == typeof(DocumentClient);
    }

    public FollowUpMessage FollowUpMessage()
    {
        return new FollowUpMessage { FollowUpText = _followUpMessage };
    }

    public ProcessMessageStatus ProcessMessage(BaseMessage baseMessage)
    {
        var message = JsonSerializer.Deserialize<GetFileMessage>(baseMessage.Payload);
        if (message != null && message.Document != null)
        {
            Console.WriteLine($"({baseMessage.SessionId}): received {message.Document.Name}");
            var directory = PathProvider.GetClientBasePath(baseMessage.SessionId.ToString());
            Directory.CreateDirectory(directory);
            var fileName = Path.Combine(directory, message.Document.NameWithExtension ?? "temp.txt");
            _currentDocument = message.Document;
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