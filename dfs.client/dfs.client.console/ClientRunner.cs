using System.Net;
using dfs.client.console.messageprocessors;
using dfs.core.common.dispatcher;
using dfs.core.common.settings;

namespace dfs.client.console;

public class ClientRunner
{
    private readonly ClientSettings _clientSettings;
    private readonly DocumentClient _client;
    public static bool run = true;
    public ClientRunner()
    {
        _clientSettings = SettingsReader.GetSettings().Client;
        var ipAddress = IPAddress.Parse(_clientSettings.IPAddress);
        _client = new DocumentClient(ipAddress, _clientSettings.Port, new MessageProvider());
    }

    public void Begin()
    {
        Console.Write("Client connecting...");
        _client.ConnectAsync();
        Console.WriteLine("Done!");
        Console.WriteLine("Starting Session...");
        while (_client._sessionId == Guid.Empty)
        {
        }
        Console.WriteLine($"Session established {_client._sessionId}");
        _client.GetDocuments();
        while (_client.IsConnected)
        {
        }
    }
}