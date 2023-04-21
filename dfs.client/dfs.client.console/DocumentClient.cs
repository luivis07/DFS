using System.Net;
using System.Text.Json;
using dfs.core.common.models;
using dfs.core.common.network;

namespace dfs.client.console;

public class DocumentClient : BaseTcpClient
{
    public DocumentClient(IPAddress address, int port) : base(address, port)
    {
    }

    protected override void OnReceived(BaseMessage baseMessage)
    {
        if (baseMessage.MessageType == MessageType.GET_ALL_FILEINFO)
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
                    Console.WriteLine($"Requested {selection.Name}");
                    SendAsync(baseMessage.Reply(fileMessage.AsJson(), MessageType.GET_FILE).AsJson());
                    Console.WriteLine($"Sent request for {baseMessage.SessionId}");
                }
                else
                {
                    End();
                }
            }
            GetDocuments();
        }
    }
    public void GetDocuments()
    {
        var baseMessage = new BaseMessage
        {
            SessionId = _sessionId,
            MessageType = MessageType.GET_ALL_FILEINFO
        };
        SendAsync(baseMessage.AsJson());
    }
    public void End()
    {
        Console.Write("Client disconnecting...");
        DisconnectAndStop();
        Console.WriteLine("Done!");
    }
}