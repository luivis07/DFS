using System.Net;
using System.Net.Sockets;
using System.Text;

var ipHostInfo = await Dns.GetHostEntryAsync("localhost");
var ipAddress = ipHostInfo.AddressList[0];

var ipEndPoint = new IPEndPoint(ipAddress, 11_000);
using Socket client = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

await client.ConnectAsync(ipEndPoint);
while (true)
{

    // Receive ack.
    var buffer = new byte[76800];
    var received = await client.ReceiveAsync(buffer, SocketFlags.None);
    if (received > 0)
    {
        File.WriteAllBytes(@"C:\Projects\DFS\dfs.documents\datastore\receive.txt", buffer);
        break;
    }
    // Sample output:
    //     Socket client sent message: "Hi friends 👋!<|EOM|>"
    //     Socket client received acknowledgment: "<|ACK|>"
}

client.Shutdown(SocketShutdown.Both);