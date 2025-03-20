using CommunityToolkit.Mvvm.Messaging.Messages;
using KnxHelden.SHES.App.ViewModels;

namespace KnxHelden.SHES.App.Messages
{
    public class AppBarSenderMessage : ValueChangedMessage<AppInfoBarViewModel>
    {
        public AppBarSenderMessage(AppInfoBarViewModel options)
            : base(options)
        {
        }
    }
}
