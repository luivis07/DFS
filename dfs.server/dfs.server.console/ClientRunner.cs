using System.Net;
using System.Text.Json;
using dfs.core.common.models;
using dfs.core.common.settings;

namespace dfs.server.console;

public class ClientRunner
{
    private readonly ServerSettings _serverSettings;
    private readonly DocumentClient _client;

    public ClientRunner()
    {
        _serverSettings = SettingsReader.GetSettings().Server;
        var ipAddress = IPAddress.Parse(_serverSettings.IPAddress);
        _client = new DocumentClient(ipAddress, _serverSettings.Port);
    }
    public void Begin()
    {
        Console.Write("Client connecting...");
        _client.ConnectAsync();
        Console.WriteLine("Done!");

        while (_client._sessionId == Guid.Empty)
        {
            
        }
        Init();
        while (true)
        {
            string line = Console.ReadLine() ?? "none";
            if (string.IsNullOrEmpty(line))
                break;
        }
        Console.Write("Client disconnecting...");
        _client.DisconnectAndStop();
        Console.WriteLine("Done!");
    }

    public void Init()
    {
        var baseMessage = new BaseMessage
        {
            SessionId = _client._sessionId,
            MessageType = MessageType.GET_ALL_FILEINFO
        };
        _client.SendAsync(baseMessage.AsJson());
    }
}