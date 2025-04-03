using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace KnxHelden.SHES.Controls.FormFieldService
{
    /// <summary>
    /// Provides helper methods for creating UI form fields with data binding.
    /// </summary>
    /// <remarks>
    /// This service simplifies the creation of common form controls such as <see cref="TextBox"/>, <see cref="ComboBox"/>, 
    /// and <see cref="CheckBox"/> by automatically setting up data bindings and optional event handlers.
    /// </remarks>
    public class FormFieldService : IFormFieldService
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
        public TextBox GetTextBox(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler)
        {
            var textBox = new TextBox();

            // Set up two-way data binding to the specified property
            textBox.SetBinding(TextBox.TextProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay
            });

            // Attach the LosingFocus event handler if provided
            if (losingFocusHandler != null)
                textBox.LosingFocus += losingFocusHandler;

            return textBox;
        }

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
        public ComboBox GetEnumComboBox<T>(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler) where T : struct, Enum
        {
            var comboBox = new ComboBox
            {
                // Populate the ComboBox with all values of the given enum type
                ItemsSource = Enum.GetValues(typeof(T)).Cast<T>().ToList()
            };

            // Define a DataTemplate for displaying the enum values with a display name converter
            string xamlTemplate =
                "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
                    "<TextBlock Text='{Binding Converter={StaticResource EnumDisplayNameConverter}}'/>" +
                "</DataTemplate>";

            // Apply the custom DataTemplate to display enum values properly
            comboBox.ItemTemplate = (DataTemplate)XamlReader.Load(xamlTemplate);

            // Set up two-way data binding to the specified property
            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay
            });

            // Attach the LosingFocus event handler if provided
            if (losingFocusHandler != null)
                comboBox.LosingFocus += losingFocusHandler;

            return comboBox;
        }

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
        public CheckBox GetCheckBox(object source, string propertyName, RoutedEventHandler clickHandler)
        {
            var checkBox = new CheckBox();

            // Set up two-way data binding to the specified property
            checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay
            });

            // Attach the Click event handler if provided
            if (clickHandler != null)
                checkBox.Click += clickHandler;

            return checkBox;
        }

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
        public ComboBox GetComboBox<T>(object source, string propertyName, TypedEventHandler<UIElement, LosingFocusEventArgs> losingFocusHandler, IEnumerable<T> itemSource, string displayMemberPath = "", string selectedValuePath = "") where T : class
        {
            var comboBox = new ComboBox
            {
                ItemsSource = itemSource,
                DisplayMemberPath = displayMemberPath
            };

            // Set up two-way data binding to the specified property
            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay
            });

            // Attach the LosingFocus event handler if provided
            if (losingFocusHandler != null)
                comboBox.LosingFocus += losingFocusHandler;

            return comboBox;
        }
    }
}
