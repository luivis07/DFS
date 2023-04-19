using System.Net;
using dfs.core.common.network;
using NetCoreServer;
namespace dfs.datastore.console;

public class DatastoreServer : TcpServer
{
    public DatastoreServer(IPAddress address, int port) : base(address, port)
    {
    }
    protected override TcpSession CreateSession()
    {
        return new CommSession(this);
    }
}