using dfs.core.common.models;

namespace dfs.core.common.dispatcher;

public interface IMessageProcessor
{
    ProcessMessageStatus ProcessMessage(BaseMessage baseMessage);
    bool AppliesTo(BaseMessage baseMessage, Type sender);
    string? FollowUpMessage();
}