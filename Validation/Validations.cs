using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace kurs.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }

    public class PasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ((value ?? "").ToString()).Length > 8
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Password minimum length is 8 characters");
        }
    }

    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return Regex.IsMatch((value ?? "").ToString(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Email is invalid.");
        }
    }
}
