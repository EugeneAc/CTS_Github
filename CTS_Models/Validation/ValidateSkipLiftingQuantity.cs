using System;
using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
	public class ValidateSkipLiftingQuantity : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{		
			if (value != null)
				try
				{
					int numericValue = Int32.Parse(value.ToString());
					if ((numericValue > 0) && (numericValue < 500))
					{
						return ValidationResult.Success;
					}
					else
					{
						return new ValidationResult("Значение должно быть от 1 до 499");
					}
				}
				catch (System.Exception ex)
				{
					return new ValidationResult("Неверный формат числа");
				}

			return ValidationResult.Success;
		}
	}
}

