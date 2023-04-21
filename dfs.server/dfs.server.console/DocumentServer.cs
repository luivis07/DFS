using System.Net;
using dfs.core.common.network;
using dfs.server.console.messageprocessors;
using NetCoreServer;

namespace dfs.server.console;

public class DocumentServer : BaseTcpServer
{
    public DocumentServer(IPAddress address, int port) : base(address, port)
    {
    }

    protected override TcpSession GetSession()
    {
        return new DocumentServerSession(this, new MessageProvider());
    }
}