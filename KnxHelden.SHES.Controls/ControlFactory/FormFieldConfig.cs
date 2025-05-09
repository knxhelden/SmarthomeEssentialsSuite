using System;
using System.Collections;

namespace KnxHelden.SHES.Controls.ControlFactory
{
    public sealed class FormFieldConfig
    {
        public string Label { get; set; }
        public string PropertyName { get; set; }
        public string PropertyPath { get; set; }
        public FormFieldType FieldType { get; set; }
        public IEnumerable ItemsSource { get; set; }
        public string DisplayMemberPath { get; set; }
        public string SelectedValuePath { get; set; }
        public Type EnumType { get; set; }
    }
}
