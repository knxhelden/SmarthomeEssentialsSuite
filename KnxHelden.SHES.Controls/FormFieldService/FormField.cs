using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace KnxHelden.SHES.Controls.FormFieldService
{
    public class FormField
    {
        public string Label { get; }

        public Control Control { get; }

        public string PropertyName { get; }

        public FormField(string label, Control control, string propertyName = "")
        {
            Label = label;
            Control = control;
            PropertyName = propertyName;
        }
    }
}
