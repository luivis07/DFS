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
                ClientUI.DisplayDocuments(message.Documents);
            }
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
}