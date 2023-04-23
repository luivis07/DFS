using dfs.core.common.helpers;
using dfs.core.common.models;
using dfs.core.common.settings;

namespace dfs.cache.console;
public static class CacheStorage
{
    private static ICollection<Document> _documents { get; set; }
    public static long UsedSpace => _documents.Sum(d => d.Size);
    public static long TotalSpace => _cacheSettings.Size;
    public static long FreeSpace => TotalSpace - UsedSpace;

    private static readonly CacheSettings _cacheSettings;

    static CacheStorage()
    {
        _documents = new List<Document>();
        _cacheSettings = SettingsReader.GetSettings().Cache;
    }
    public static bool Add(Document document, byte[] contents)
    {
        if (HasEnoughFreeSpace(contents))
        {
            var targetDir = PathProvider.GetCacheBasePath();
            document.FullPath = Path.Combine(targetDir, document.NameWithExtension);
            _documents.Add(document);
            File.WriteAllBytes(document.FullPath, contents);
            return true;
        }
        return false;
    }

    public static bool Exists(string name)
    {
        return _documents.Any(d => string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    private static bool HasEnoughFreeSpace(byte[] contents)
    {
        var usedSpace = _documents.Sum(d => d.Size) + contents.Length;
        return usedSpace <= _cacheSettings.Size;
    }

    public static void ClearCache()
    {
        var directory = new DirectoryInfo(PathProvider.GetCacheBasePath());
        foreach (var f in directory.GetFiles())
        {
            f.Delete();
        }
    }

    public static byte[] GetDocumentContent(string name)
    {
        if (Exists(name))
        {
            var document = _documents.First(d => string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase));
            var bytes = File.ReadAllBytes(document.FullPath);
            return bytes;
        }
        return new byte[0];
    }

    public static void RemoveDocument(string name)
    {
        var document = _documents.FirstOrDefault(d => string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase));
        if (document == null)
            return;

        _documents.Remove(document);
        File.Delete(document.FullPath);
    }
}