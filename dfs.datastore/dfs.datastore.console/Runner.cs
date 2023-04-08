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
}