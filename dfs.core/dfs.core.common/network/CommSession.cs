using System.Text;
using System.Text.Json;
using dfs.core.common.models;
using NetCoreServer;

namespace dfs.core.common.network;

public class CommSession : TcpSession
{
    public CommSession(TcpServer server) : base(server)
    {
    }

    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        var message = Encoding.UTF8.GetString(buffer, (int) offset, (int) size);
        Console.WriteLine(message);
        base.OnReceived(buffer, offset, size);
    }
}