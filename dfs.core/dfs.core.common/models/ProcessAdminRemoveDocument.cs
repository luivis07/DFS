using System.Text.Json;

namespace dfs.core.common.models;

public class ProcessAdminRemoveDocument : IAdminMessage
{
    public Document? Document { get; set; }

    public byte[]? GetFollowUpContent()
    {
        return null;
    }

    public string GetMessageType()
    {
        return MessageType.ADMIN_REMOVE_DOCUMENT;
    }
}