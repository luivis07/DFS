using dfs.core.common.settings;
namespace dfs.core.common.tests;

[TestClass]
public class SettingsReaderTests
{
    [TestMethod]
    public async Task GetSettings_Matches_File()
    {
        var settings = await SettingsReader.GetSettings();
        Assert.IsNotNull(settings);
        Assert.IsNotNull(settings.Datastore);
        Assert.IsNotNull(settings.Cache);
        Assert.IsNotNull(settings.Server);
        Assert.AreEqual(6, settings.Server.Documents.Count());
    }
}