using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace KnxHelden.SHES.Controls
{
    public class FormField
    {
        public string Label { get; }
        public Control Control { get; }

        public FormField(string label, Control control)
        {
            Label = label;
            Control = control;
        }
    }
}
