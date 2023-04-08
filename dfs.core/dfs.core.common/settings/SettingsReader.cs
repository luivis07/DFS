using System.Text.Json;
using dfs.core.common.helpers;

namespace dfs.core.common.settings;
public static class SettingsReader
{
    private static string APP_SETTINGS_PATH => @"appsettings.json";
    public static SettingsModel GetSettings()
    {
        var finalLocation = Path.Combine(PathProvider.GetSolutionBasePath(), APP_SETTINGS_PATH);
        var settingsText = File.ReadAllText(finalLocation);
        return JsonSerializer.Deserialize<SettingsModel>(settingsText) ?? new SettingsModel();
    }
}