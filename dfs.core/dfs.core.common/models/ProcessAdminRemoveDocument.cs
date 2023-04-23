using System.Text.Json;

namespace dfs.core.common.models;

public class ProcessAdminRemoveDocument : IAdminMessage
{
    public Document? Document { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public byte[]? GetFollowUpContent()
    {
        return null;
    }

    public string GetMessageType()
    {
        return MessageType.ADMIN_REMOVE_DOCUMENT;
    }
}