using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models
{
        public class ValidateDate : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                // your validation logic
                if ((DateTime)value <= DateTime.Now)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Неправильная дата - нельзя внести данные в будущее");
                }
            }
        }
}