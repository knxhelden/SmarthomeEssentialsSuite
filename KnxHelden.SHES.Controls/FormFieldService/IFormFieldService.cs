using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using Windows.Foundation;

namespace KnxHelden.SHES.Controls.FormFieldService
{
    public interface IFormFieldService
    {
        TextBox GetTextBox(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler);
        ComboBox GetEnumComboBox<T>(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler) where T : struct, Enum;
        CheckBox GetCheckBox(object source, string propertyName, RoutedEventHandler clickHandler);
    }
}
