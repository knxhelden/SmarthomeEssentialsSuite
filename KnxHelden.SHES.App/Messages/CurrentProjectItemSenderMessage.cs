using CommunityToolkit.Mvvm.Messaging.Messages;
using KnxHelden.SHES.Models.Observables;

namespace KnxHelden.SHES.App.Messages
{
    public sealed class CurrentProjectItemSenderMessage : ValueChangedMessage<ObservableProjectItem>
    {
        public CurrentProjectItemSenderMessage(ObservableProjectItem projectItem)
            : base(projectItem)
        {
        }
    }
}
