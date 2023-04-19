namespace dfs.core.common.models;

public class DocumentTransfer
{
    public string NameWithExtension { get; set; } = string.Empty;
    public string Name => Path.GetFileNameWithoutExtension(NameWithExtension);
    public byte[] Contents { get; set; } = Array.Empty<byte>();
}