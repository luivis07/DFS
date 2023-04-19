using System.Net;
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

            Console.WriteLine("Press Enter to stop the client or '!' to reconnect the client...");

            // Perform text input
            for (;;)
            {
                string line = Console.ReadLine() ?? "none";
                if (string.IsNullOrEmpty(line))
                    break;

                // Disconnect the client
                if (line == "!")
                {
                    Console.Write("Client disconnecting...");
                    _client.DisconnectAsync();
                    Console.WriteLine("Done!");
                    continue;
                }

                // Send the entered text to the chat server
                _client.SendAsync(line);
            }

            // Disconnect the client
            Console.Write("Client disconnecting...");
            _client.DisconnectAndStop();
            Console.WriteLine("Done!");
    }
}