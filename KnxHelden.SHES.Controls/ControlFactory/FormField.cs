using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace KnxHelden.SHES.Controls.ControlFactory
{
    public class FormField
    {
        public string Label { get; }

        public Control Control { get; }

        public string PropertyPath { get; }

        public FormField(FormFieldConfig config, object source)
        {
            Label = config.Label;
            PropertyPath = config.PropertyPath;
            Control = ControlFactory.CreateControl(config, source);
        }
    }
}
