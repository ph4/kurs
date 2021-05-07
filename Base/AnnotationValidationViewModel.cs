using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace kurs
{
    /// <summary>
    /// Basic ViewModel class with <c>DataAnnotations</c> validation
    /// </summary>
    public class AnnotationValidationViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, PropertyInfo> _Properties;
        private readonly Dictionary<string, List<object>> _ValidationErrorsByProperty =
            new Dictionary<string, List<object>>();

        public bool ValidateModelOnProperyChanged { get; set; } = false;

        protected AnnotationValidationViewModel()
        {
            _Properties = GetType().GetProperties().ToDictionary(x => x.Name);
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (ValidateModelOnProperyChanged)
                ValidateModel();
            else
                ValidateProperty(propertyName);
        }

        public bool ValidateModel()
        {
            bool rv = true;
            foreach (string propertyName in _Properties.Keys)
            {
                rv &= ValidateProperty(propertyName);
            }
            return rv;
        }

        public bool ValidateProperty(string propertyName)
        {
            if (_Properties.TryGetValue(propertyName, out PropertyInfo propInfo))
            {
                var errors = new List<object>();
                foreach (var attribute in propInfo.GetCustomAttributes<ValidationAttribute>())
                {
                    if (attribute.RequiresValidationContext)
                    {
                        var validationCtx = new ValidationContext(this)
                        {
                            // https://stackoverflow.com/questions/57427245/testing-custom-validation-of-property-depenent-on-other-property-returns-argumen
                            MemberName = propInfo.Name
                        };
                        var validationResult = attribute.GetValidationResult(propInfo.GetValue(this), validationCtx);
                        if (validationResult != null)
                            errors.Add(validationResult.ErrorMessage);
                    } else
                    {
                        if (!attribute.IsValid(propInfo.GetValue(this)))
                        {
                            errors.Add(attribute.FormatErrorMessage(propertyName));
                        }
                    }
                }

                if (errors.Any())
                {
                    _ValidationErrorsByProperty[propertyName] = errors;
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                    return false;
                }
                if (_ValidationErrorsByProperty.Remove(propertyName))
                {
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }

            return true;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (_ValidationErrorsByProperty.TryGetValue(propertyName, out List<object> errors))
            {
                return errors;
            }
            return Array.Empty<object>();
        }

        public List<string> GetErrorsString(string propertyName)
        {
            return GetErrors(propertyName).Cast<string>().ToList();
        }

        public bool HasErrors => _ValidationErrorsByProperty.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
