using System.Net;
using System.Text;
using System.Text.Json;
using dfs.core.common.models;
using dfs.core.common.network;
using NetCoreServer;

namespace dfs.server.console;

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
            ServerStorage.SetFileInfo(message);
        }
    }
}