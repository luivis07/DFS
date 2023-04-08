namespace dfs.core.common.models;
public class Document
{
    public string NameWithExtension { get; set; } = string.Empty;
    public int Cost { get; set; }
    public int QuantityAvailable { get; set; }
    public long Size { get; set; }
    public string FullPath { get; set; } = string.Empty;
    public string Name => Path.GetFileNameWithoutExtension(NameWithExtension);
}