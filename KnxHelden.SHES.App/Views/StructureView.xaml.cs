using KnxHelden.SHES.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace KnxHelden.SHES.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StructureView : Page
    {
        public StructureViewModel ViewModel { get; }

        public StructureView()
        {
            ViewModel = App.Services.GetService<StructureViewModel>();
            this.InitializeComponent();
        }
    }
}
