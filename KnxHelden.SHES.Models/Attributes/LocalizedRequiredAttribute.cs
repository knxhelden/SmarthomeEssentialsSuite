using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace KnxHelden.SHES.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedRequiredAttribute : ValidationAttribute
    {
        private readonly string _resourceKey;

        public LocalizedRequiredAttribute(string resourceKey)
        {
            _resourceKey = resourceKey;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            // Wert ungültig?
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                // .resw-Ressource auslesen
                var loader = context.GetService(typeof(ResourceLoader)) as ResourceLoader;
                var template = loader.GetString(_resourceKey);
                // Fallback, falls kein Eintrag
                if (string.IsNullOrEmpty(template))
                    template = $"The {context.DisplayName} field is required.";

                // Platzhalter ersetzen
                var message = string.Format(template, context.DisplayName);
                return new ValidationResult(message, new[] { context.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}
