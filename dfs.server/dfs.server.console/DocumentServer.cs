using System.Net;
using dfs.core.common.network;
using dfs.server.console.messageprocessors;
using dfs.server.console.recovery;
using NetCoreServer;

namespace dfs.server.console;

public class DocumentServer : BaseTcpServer
{
    public RecoveryManager RecoveryManager {get; set;}
    public DocumentServer(IPAddress address, int port) : base(address, port)
    {
        RecoveryManager = new RecoveryManager();
    }

    protected override TcpSession GetSession()
    {
        return new DocumentServerSession(this, new MessageProvider());
    }
}