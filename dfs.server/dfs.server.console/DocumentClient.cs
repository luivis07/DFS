using System.Net;
using System.Text;
using NetCoreServer;

namespace dfs.server.console;

public class DocumentClient : TcpClient
{
    private bool _stop;
    public DocumentClient(IPAddress address, int port) : base(address, port)
    {
    }
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        var message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
        Console.WriteLine(message);
    }

    public void DisconnectAndStop()
    {
        _stop = true;
        DisconnectAsync();
        while (IsConnected)
            Thread.Yield();
    }
}