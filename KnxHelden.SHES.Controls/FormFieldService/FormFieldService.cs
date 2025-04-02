using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace KnxHelden.SHES.Controls.FormFieldService
{
    public class FormFieldService : IFormFieldService
    {
        public TextBox GetTextBox(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler)
        {
            var textBox = new TextBox();
            textBox.SetBinding(TextBox.TextProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay
            });

            if (losingFocusHandler != null)
                textBox.LosingFocus += losingFocusHandler;

            return textBox;
        }

        public ComboBox GetEnumComboBox<T>(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler) where T : struct, Enum
        {
            var comboBox = new ComboBox
            {
                ItemsSource = Enum.GetValues(typeof(T)).Cast<T>().ToList()
            };

            string xamlTemplate =
                "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
                    "<TextBlock Text='{Binding Converter={StaticResource EnumDisplayNameConverter}}'/>" +
                "</DataTemplate>";

            comboBox.ItemTemplate = (DataTemplate)XamlReader.Load(xamlTemplate);

            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay
            });

            if (losingFocusHandler != null)
                comboBox.LosingFocus += losingFocusHandler;

            return comboBox;
        }

        public CheckBox GetCheckBox(object source, string propertyName, RoutedEventHandler clickHandler)
        {
            var checkBox = new CheckBox();
            checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay
            });

            if (clickHandler != null)
                checkBox.Click += clickHandler;

            return checkBox;
        }

        public ComboBox GetComboBox<T>(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler, IEnumerable<T> itemSource, string displayMemberPath = "", string selectedValuePath = "") where T : class
        {
            var comboBox = new ComboBox
            {
                ItemsSource = itemSource,
                DisplayMemberPath = displayMemberPath
            };

            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay
            });

            if (losingFocusHandler != null)
                comboBox.LosingFocus += losingFocusHandler;

            return comboBox;
        }
    }
}
