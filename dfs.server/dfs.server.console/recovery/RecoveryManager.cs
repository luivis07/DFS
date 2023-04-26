using System.Text.Json;
using dfs.core.common.helpers;
using dfs.core.common.models;

namespace dfs.server.console.recovery;

public class RecoveryManager
{
    public bool Save(BaseMessage baseMessage)
    {
        try
        {
            var text = baseMessage.AsJson();
            var filePath = Path.Combine(PathProvider.GetStableStoragePath(), $"{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff")}.json");
            File.WriteAllText(filePath, text);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public BaseMessage? GetRecoveryCheckpoint()
    {
        var directory = new DirectoryInfo(PathProvider.GetStableStoragePath());
        var mostRecent = directory.GetFiles("*.json").OrderByDescending(f => f.LastWriteTimeUtc).FirstOrDefault();
        if (mostRecent == null)
            return null;
        var baseMessage = File.ReadAllText(mostRecent.FullName);
        return JsonSerializer.Deserialize<BaseMessage>(baseMessage);
    }
}