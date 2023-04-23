using System.Text.Json;

namespace dfs.core.common.models;

public class ProcessAdminAddDocument : IAdminMessage
{
    public byte[]? FollowUpContent { get; set; }
    public Document? Document { get; set; }

    public byte[]? GetFollowUpContent()
    {
        return FollowUpContent;
    }

    public string GetMessageType()
    {
        return MessageType.ADMIN_ADD_DOCUMENT;
    }
}