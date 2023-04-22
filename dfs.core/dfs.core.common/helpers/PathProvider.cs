using dfs.core.common.settings;
namespace dfs.core.common.helpers;
public static class PathProvider
{
    private static string SOLUTION_BASE_DIRECTORY = @"DFS";
    public static string GetSolutionBasePath()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var rootFolder = baseDirectory.IndexOf(@"DFS\");
        var relativePath = baseDirectory.Substring(0, rootFolder);
        return Path.Combine(relativePath, SOLUTION_BASE_DIRECTORY);
    }
    public static string GetDatastoreBasePath()
    {
        var settings = SettingsReader.GetSettings();
        return Path.Combine(GetSolutionBasePath(), settings.Datastore.Path);
    }
    public static string GetClientBasePath(string clientSpecific = "")
    {
        var settings = SettingsReader.GetSettings();
        return clientSpecific == string.Empty ?
            Path.Combine(GetSolutionBasePath(), settings.Client.Path) :
            Path.Combine(GetSolutionBasePath(), settings.Client.Path, clientSpecific);
    }

    public static string GetCacheBasePath()
    {
        var settings = SettingsReader.GetSettings();
        return Path.Combine(GetSolutionBasePath(), settings.Cache.Path);
    }
}