using dfs.core.common.helpers;
using dfs.core.common.models;

namespace dfs.datastore.console;
public class Runner
{
    private static string BASE_PATH => PathProvider.GetDatastoreBasePath();
    public IEnumerable<string> GetFiles()
    {
        var fileEntries = Directory.GetFiles(BASE_PATH);
        return fileEntries;
    }

    public IEnumerable<Document> GetFileInfo()
    {
        var filePaths = GetFiles();
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
}