using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
  public class ValidateWeight : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      try
      {
        if ((value != null) && ((float)value > 0))
        {
          return ValidationResult.Success;
        }
        else
        {
          return new ValidationResult("Неправильный вес");
        }
      }
      catch (System.Exception ex)
      {
        return new ValidationResult("Неверный формат числа");
      }
    }
  }
}

