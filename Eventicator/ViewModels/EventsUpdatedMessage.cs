using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Eventicator.ViewModels
{
    public sealed class EventsUpdatedMessage : ValueChangedMessage<bool>
    {
        public EventsUpdatedMessage() : base(true)
        {
        }
    }
}
