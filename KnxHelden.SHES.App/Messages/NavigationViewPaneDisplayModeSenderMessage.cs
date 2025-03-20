using CommunityToolkit.Mvvm.Messaging.Messages;
using KnxHelden.SHES.Models.Observables;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.Messages
{
    class NavigationViewPaneDisplayModeSenderMessage : ValueChangedMessage<NavigationViewPaneDisplayMode>
    {
        public NavigationViewPaneDisplayModeSenderMessage(NavigationViewPaneDisplayMode navigationViewPaneDisplayMode)
            : base(navigationViewPaneDisplayMode)
        {
        }
    }
}
