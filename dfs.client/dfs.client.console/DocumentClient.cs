using System.Net;
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
        throw new NotImplementedException();
    }
}