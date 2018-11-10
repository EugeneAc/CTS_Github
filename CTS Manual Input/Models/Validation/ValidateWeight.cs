using System.ComponentModel.DataAnnotations;

namespace CTS_Manual_Input.Models.Validation
{
    public class ValidateWeight : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((float)value > 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Неправильный вес - должен быть больше 0");
            }
        }
    }
}

