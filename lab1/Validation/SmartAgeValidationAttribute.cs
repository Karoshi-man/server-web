using System;
using System.ComponentModel.DataAnnotations;

namespace lab1.Validation 
{
    public class SmartAgeValidationAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;
        private readonly int _maximumAge;

        public SmartAgeValidationAttribute(int minimumAge, int maximumAge)
        {
            _minimumAge = minimumAge;
            _maximumAge = maximumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                if (dateOfBirth > DateTime.Now)
                {
                    return new ValidationResult("Date of Birth cannot be in the future.");
                }

                var age = DateTime.Today.Year - dateOfBirth.Year;
                if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < _minimumAge)
                {
                    return new ValidationResult(ErrorMessage ?? $"You must be at least {_minimumAge} years old.");
                }

                if (age > _maximumAge)
                {
                    return new ValidationResult($"Please enter a valid year of birth. (Max age is {_maximumAge})");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date format.");
        }
    }
}