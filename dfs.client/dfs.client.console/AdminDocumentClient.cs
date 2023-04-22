using System.Net;
using dfs.client.console.messageprocessors;
using dfs.core.common.models;
using dfs.core.common.network;

namespace dfs.client.console;

public class AdminDocumentClient : BaseTcpClient
{
    private readonly AdminMessageProvider _adminMessageProvider;

    public AdminDocumentClient(IPAddress address, int port, AdminMessageProvider adminMessageProvider) : base(address, port)
    {
        _adminMessageProvider = adminMessageProvider;
    }

    protected override void OnReceived(BaseMessage baseMessage)
    {
        throw new NotImplementedException();
    }
}