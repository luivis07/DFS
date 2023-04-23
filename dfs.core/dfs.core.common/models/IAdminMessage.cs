namespace dfs.core.common.models
{
    public interface IAdminMessage
    {
        string GetMessageType();
        byte[]? GetFollowUpContent();
        Document? Document { get; set; }
    }
}