using System;
using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
  public class ValidateDate : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value != null)
      {
        // your validation logic
        if ((DateTime)value <= DateTime.Now)
        {
          return ValidationResult.Success;
        }
        else
        {
          return new ValidationResult("Неправильная дата");
        }
      }
      else
        return new ValidationResult("Ошибка конвертации даты");
    }
  }
}