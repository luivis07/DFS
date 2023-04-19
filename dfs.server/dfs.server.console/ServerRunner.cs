using System.Net;
using dfs.core.common.settings;

namespace dfs.server.console;

public class ServerRunner
{
    private readonly ServerSettings _server;
    private readonly DocumentServer _documentServer;

    public ServerRunner()
    {
        _server = SettingsReader.GetSettings().Server;
        _documentServer = new DocumentServer(IPAddress.Any, _server.Port);
    }
    public void Begin()
    {
        Console.Write("Server starting...");
        _documentServer.Start();
        Console.WriteLine("Done!");

        while (true)
        {
            string line = Console.ReadLine() ?? "none";
            if (string.IsNullOrEmpty(line))
                break;
        }

        Console.Write("Server stopping...");
        _documentServer.Stop();
        Console.WriteLine("Done!");
    }
}