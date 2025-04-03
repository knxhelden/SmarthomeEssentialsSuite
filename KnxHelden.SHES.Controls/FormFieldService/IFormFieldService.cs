using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using Windows.Foundation;

namespace KnxHelden.SHES.Controls.FormFieldService
{
    public interface IFormFieldService
    {
        /// <summary>
        /// Creates and returns a <see cref="TextBox"/> with a two-way data binding to a specified property.
        /// </summary>
        /// <param name="source">The source object that contains the property to bind.</param>
        /// <param name="propertyName">The name of the property to bind to the <see cref="TextBox.Text"/> property.</param>
        /// <param name="losingFocusHandler">
        /// An optional event handler for the <see cref="UIElement.LosingFocus"/> event.
        /// If provided, the handler is attached to the <see cref="TextBox"/>.
        /// </param>
        /// <returns>
        /// A <see cref="TextBox"/> instance with a two-way binding to the specified property and an optional event handler.
        /// </returns>
        TextBox GetTextBox(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler);

        /// <summary>
        /// Creates and returns a <see cref="ComboBox"/> for selecting values from an enumeration.
        /// </summary>
        /// <typeparam name="T">The enumeration type to populate the <see cref="ComboBox"/> with.</typeparam>
        /// <param name="source">The source object that contains the property to bind.</param>
        /// <param name="propertyName">The name of the property to bind to the <see cref="ComboBox.SelectedItem"/> property.</param>
        /// <param name="losingFocusHandler">
        /// An optional event handler for the <see cref="UIElement.LosingFocus"/> event.
        /// If provided, the handler is attached to the <see cref="ComboBox"/>.
        /// </param>
        /// <returns>
        /// A <see cref="ComboBox"/> populated with the values of the specified enumeration type
        /// and a two-way binding to the specified property.
        /// </returns>
        ComboBox GetEnumComboBox<T>(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler) where T : struct, Enum;

        /// <summary>
        /// Creates and returns a <see cref="CheckBox"/> with a two-way data binding.
        /// </summary>
        /// <param name="source">The source object that contains the property to bind.</param>
        /// <param name="propertyName">The name of the property to bind to the <see cref="CheckBox.IsChecked"/> property.</param>
        /// <param name="clickHandler">
        /// An optional event handler for the <see cref="CheckBox.Click"/> event.
        /// If provided, the handler is attached to the <see cref="CheckBox"/>.
        /// </param>
        /// <returns>
        /// A <see cref="CheckBox"/> with a two-way binding to the specified property.
        /// </returns>
        ComboBox GetComboBox<T>(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler, IEnumerable<T> itemSource, string displayMemberPath = "", string selectedValuePath = "") where T : class;

        /// <summary>
        /// Creates and returns a <see cref="ComboBox"/> with a two-way data binding.
        /// </summary>
        /// <typeparam name="T">The type of items in the provided <paramref name="itemSource"/>.</typeparam>
        /// <param name="source">The source object that contains the property to bind.</param>
        /// <param name="propertyName">The name of the property to bind to the <see cref="ComboBox.SelectedItem"/> property.</param>
        /// <param name="losingFocusHandler">
        /// An optional event handler for the <see cref="UIElement.LosingFocus"/> event.
        /// If provided, the handler is attached to the <see cref="ComboBox"/>.
        /// </param>
        /// <param name="itemSource">The collection of items to be displayed in the <see cref="ComboBox"/>.</param>
        /// <param name="displayMemberPath">
        /// The name of the property to display in the <see cref="ComboBox"/> when binding complex objects.
        /// If left empty, the default string representation of the object is used.
        /// </param>
        /// <param name="selectedValuePath">
        /// (Optional) Specifies the path to a property on the bound objects that represents the selected value.
        /// </param>
        /// <returns>
        /// A <see cref="ComboBox"/> bound to the specified property, populated with the provided items.
        /// </returns>
        CheckBox GetCheckBox(object source, string propertyName, RoutedEventHandler clickHandler);
    }
}
