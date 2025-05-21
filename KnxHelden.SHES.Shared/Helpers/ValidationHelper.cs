using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Shared.Helpers
{
    /// <summary>
    /// Provides utility methods for handling validation-related logic,
    /// including detecting validation attributes on properties and conditionally
    /// invoking update operations based on property change events.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Checks if the specified property of type <typeparamref name="T"/> has a ValidationAttribute.
        /// </summary>
        /// <typeparam name="T">The type that contains the property.</typeparam>
        /// <param name="propertyName">The name of the property to check.</param>
        /// <returns>True if the property has a validation attribute; otherwise, false.</returns>
        public static bool HasValidationAttribute<T>(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property == null) return false;

            return property.GetCustomAttributes(typeof(ValidationAttribute), true).Any();
        }

        /// <summary>
        /// Handles the PropertyChanged event by determining whether the observable object should be updated.
        /// The update occurs only if the property is not a validation-related property and there are no current errors,
        /// or if the error state has changed and there are now no errors.
        /// </summary>
        /// <typeparam name="T">The type of the observable object.</typeparam>
        /// <param name="sender">The event sender (typically the observable object).</param>
        /// <param name="e">The PropertyChangedEventArgs containing the changed property name.</param>
        /// <param name="observable">The observable object instance.</param>
        /// <param name="updateFunc">A function to call to perform the update (e.g., a service call).</param>
        public static async Task HandlePropertyChangedAsync<T>(
            object sender,
            PropertyChangedEventArgs e,
            T observable,
            Func<T, Task> updateFunc)
            where T : INotifyPropertyChanged
        {
            bool isValidationProperty = ValidationHelper.HasValidationAttribute<T>(e.PropertyName);
            bool isErrorProperty = e.PropertyName == nameof(INotifyDataErrorInfo.HasErrors) || e.PropertyName == "Errors";

            if (!isValidationProperty && !isErrorProperty && !HasErrors(observable))
            {
                await updateFunc(observable);
                return;
            }

            if (e.PropertyName == nameof(INotifyDataErrorInfo.HasErrors) && !HasErrors(observable))
            {
                await updateFunc(observable);
            }
        }

        /// <summary>
        /// Checks whether the given object has any validation errors by evaluating INotifyDataErrorInfo.HasErrors.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to check for errors.</param>
        /// <returns>True if the object has errors; otherwise, false.</returns>
        private static bool HasErrors<T>(T obj)
        {
            return obj is INotifyDataErrorInfo errorInfo && errorInfo.HasErrors;
        }
    }
}
