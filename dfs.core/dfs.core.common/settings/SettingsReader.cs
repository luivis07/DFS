using System.Text.Json;

namespace dfs.core.common.settings;
public static class SettingsReader
{
    private static string APP_SETTINGS_PATH => @"DFS\appsettings.json";
    public static SettingsModel GetSettings()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var rootFolder = baseDirectory.IndexOf(@"DFS\");
        var relativePath = baseDirectory.Substring(0, rootFolder);
        var finalLocation = Path.Combine(relativePath, APP_SETTINGS_PATH);
        var settingsText = File.ReadAllText(finalLocation);
        return JsonSerializer.Deserialize<SettingsModel>(settingsText) ?? new SettingsModel();
    }
}