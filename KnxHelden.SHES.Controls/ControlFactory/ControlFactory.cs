using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using System;

namespace KnxHelden.SHES.Controls.ControlFactory
{
    public static class ControlFactory
    {
        public static Control CreateControl(FormFieldConfig config, object source)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            Binding binding = new Binding
            {
                Source = source,
                Path = new PropertyPath(config.PropertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
            };

            switch (config.FieldType)
            {
                case FormFieldType.TextBox:
                    var textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty, binding);
                    return textBox;

                case FormFieldType.NumberBox:
                    var numberBox = new NumberBox
                    {
                        SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline,
                        SmallChange = 1
                    };
                    numberBox.SetBinding(NumberBox.ValueProperty, binding);
                    return numberBox;

                case FormFieldType.CheckBox:
                    var checkBox = new CheckBox();
                    checkBox.SetBinding(CheckBox.IsCheckedProperty, binding);
                    return checkBox;

                case FormFieldType.ComboBox:
                    var comboBox = new ComboBox
                    {
                        ItemsSource = config.ItemsSource,
                        DisplayMemberPath = config.DisplayMemberPath,
                        SelectedValuePath = config.SelectedValuePath
                    };
                    comboBox.SetBinding(ComboBox.SelectedItemProperty, binding);
                    return comboBox;

                case FormFieldType.EnumComboBox:
                    if (config.EnumType == null || !config.EnumType.IsEnum)
                        throw new ArgumentException("EnumType must be a valid enum type for EnumComboBox.");

                    var enumComboBox = new ComboBox
                    {
                        // Populate the ComboBox with all values of the given enum type
                        ItemsSource = Enum.GetValues(config.EnumType)
                    };

                    // Define a DataTemplate for displaying the enum values with a display name converter
                    string xamlTemplate =
                        "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
                            "<TextBlock Text='{Binding Converter={StaticResource EnumDisplayNameConverter}}'/>" +
                        "</DataTemplate>";

                    // Apply the custom DataTemplate to display enum values properly
                    enumComboBox.ItemTemplate = (DataTemplate)XamlReader.Load(xamlTemplate);

                    // Set up two-way data binding to the specified property
                    enumComboBox.SetBinding(ComboBox.SelectedItemProperty, binding);

                    return enumComboBox;

                default:
                    throw new NotSupportedException($"FieldType '{config.FieldType}' is not supported.");
            }
        }
    }
}
