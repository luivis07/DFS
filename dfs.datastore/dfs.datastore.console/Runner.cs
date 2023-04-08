using System.Net;
using System.Net.Sockets;
using System.Text;
using dfs.core.common.helpers;
using dfs.core.common.models;

namespace dfs.datastore.console;
public class Runner
{
    private static async Task<string> BASE_PATH() => await PathProvider.GetDatastoreBasePath();
    public async Task<IEnumerable<string>> GetFiles()
    {
        var fileEntries = Directory.GetFiles(await BASE_PATH());
        return fileEntries;
    }

    public async Task<IEnumerable<Document>> GetFileInfo()
    {
        var filePaths = await GetFiles();
        var result = new List<Document>();
        foreach (var path in filePaths)
        {
            var temp = new FileInfo(path);
            result.Add(new Document
            {
                NameWithExtension = temp.Name,
                Size = temp.Length,
                FullPath = temp.FullName
            });
        }
        return result;
    }

    public bool FileExists(string fullPath)
    {
        return File.Exists(fullPath);
    }

    public async Task<IEnumerable<byte>> GetFileContents(string fullPath)
    {
        if (FileExists(fullPath))
        {
            return await File.ReadAllBytesAsync(fullPath);
        }
        return Enumerable.Empty<byte>();
    }

    public async Task Begin()
    {
        var ipHostInfo = await Dns.GetHostEntryAsync("localhost");
        var ipAddress = ipHostInfo.AddressList[0];

        var ipEndPoint = new IPEndPoint(ipAddress, 11_000);

        using var listener = new Socket(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        listener.Bind(ipEndPoint);
        listener.Listen(100);

        var handler = await listener.AcceptAsync();
        var fileContent = (await GetFileContents(@"C:\Projects\DFS\dfs.documents\datastore\A.txt")).ToArray();
        await handler.SendAsync(fileContent, SocketFlags.None);
    }
}