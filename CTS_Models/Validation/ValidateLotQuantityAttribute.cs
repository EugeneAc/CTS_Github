using System;
using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
  public class ValidateLotQuantity : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      try
      {
        if ((float)value >= 0)
        {
          return ValidationResult.Success;
        }
        else
        {
          return new ValidationResult("Вес должен быть > 0");
        }
      }
      catch (Exception ex)
      {
        return new ValidationResult("Неправильный формат числа");
      }
     
    }
  }
}