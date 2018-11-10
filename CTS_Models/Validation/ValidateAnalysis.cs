using System.ComponentModel.DataAnnotations;

namespace CTS_Models
{
	public class ValidateAnalysis : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			
			if (value != null)
				try
				{
					if ((float)value > 0)
					{
						return ValidationResult.Success;
					}
					else
					{
						return new ValidationResult("Неверное значение анализа");
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

