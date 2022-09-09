using System.ComponentModel.DataAnnotations;

namespace TypeCode.Wpf.Helper.Validation;

public static class CustomIntValidation
{
    public static ValidationResult? ValidateInt(string? value, ValidationContext context)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        return int.TryParse(value, out _)
            ? ValidationResult.Success
            : new ValidationResult("Value has to be a number (int).");
    }
}