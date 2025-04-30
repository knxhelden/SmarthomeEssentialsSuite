using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Models.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class KnxPhysicalAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is not string input || string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var parts = input.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int area) || area < 0 || area > 15)
            {
                return false;
            }

            if (!int.TryParse(parts[1], out int line) || line < 0 || line > 15)
            {
                return false;
            }

            if (!int.TryParse(parts[2], out int address) || address < 0 || address > 255)
            {
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} muss im Format X.X.X sein, wobei Area und Line zwischen 0 und 15, und Address zwischen 0 und 255 liegen.";
        }
    }
}
