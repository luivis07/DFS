using dfs.core.common.models;

namespace dfs.core.common.dispatcher;

public interface IMessageProcessor
{
    ProcessMessageStatus ProcessMessage(BaseMessage baseMessage);
    ProcessMessageStatus ProcessMessage(byte[] buffer);
    bool AppliesTo(BaseMessage baseMessage, Type sender);
    FollowUpMessage FollowUpMessage();
}