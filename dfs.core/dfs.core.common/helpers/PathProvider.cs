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
        return Path.Combine(GetSolutionBasePath(), SettingsReader.GetSettings().Datastore.Path);
    }
}