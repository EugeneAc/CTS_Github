using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models
{
    public class ValidateNonNegative : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic
            if ((float)value >= 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Значение должно быть больше 0");
            }
        }
    }
}
