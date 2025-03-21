using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KnxHelden.SHES.Controls.Converters
{
    public class EnumDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Enum enumValue)
            {
                var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                var displayAttribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>();
                return displayAttribute?.Name ?? enumValue.ToString();
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType.IsEnum)
            {
                foreach (var field in targetType.GetFields())
                {
                    var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                    if (displayAttribute != null && displayAttribute.Name == value.ToString())
                    {
                        return Enum.Parse(targetType, field.Name);
                    }
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}