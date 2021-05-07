using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace kurs
{
    class FioAttribue : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var str = value.ToString();
            var fio = str.Trim().Split();
            return (fio.Length == 2 || fio.Length == 3) && fio.All(s => s.All(char.IsLetter));
        }
    }

    class RussianPhoneAttribute : ValidationAttribute
    {
        static readonly Regex regex = new Regex(@"(?<=\+7|7|8)(\d{10})");
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return regex.IsMatch(value.ToString());
        }
    }

    public enum UserTypes
    {
        Unknown,
        Admin,
        User,
        Manager,
    } 

    public class UserData : AnnotationValidationViewModel
    {
        private readonly Dns2Entities _dbCtx;
        private user _dbUser;
        private credentials _credentials;

        public bool IsNewUser => _dbUser.id == 0;
        public bool CanSave => _dbUser != null;
        
        public UserData() {
            _credentials = new credentials
            {
                user_type = "user"
            };

            _dbUser = new user
            {
                credentials1 = _credentials
            };
        }

        public UserData(user dbUser)
        {
            _dbUser = dbUser;
            _credentials = dbUser.credentials1;
        }

        public UserData(Dns2Entities ctx) : this()
        {
            _dbCtx = ctx;
        }

        public UserData(Dns2Entities ctx, user dbUser) {
            _dbUser = dbUser;
            _dbCtx = ctx;
        }

        public bool SaveToDb() {
            if (!CanSave) return false;

            _dbUser.credentials1 = _credentials;
            if (IsNewUser)
                _dbCtx.user.Add(_dbUser);
            _dbCtx.SaveChanges();
            System.Diagnostics.Debug.Assert(_dbUser.id != 0);
            System.Diagnostics.Debug.Assert(_credentials.id != 0);
            return true;
        }

        [Required]
        public UserTypes? UserType {
            get {
                if (_credentials.user_type == null) return null;
                return Enum.TryParse(_credentials.user_type, true, out UserTypes res)
                    ? res : UserTypes.Unknown;
            }
            set => _credentials.user_type = value?.ToString()?.ToLower();
        }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Login {
            get => _credentials.login;
            set => _credentials.login = value;
        }

        [Required]
        [MinLength(8, ErrorMessage = "Password must me at least 8 chars")]
        public string Password {
            get => _credentials.password;
            set => _credentials.password = value;
        }

        public string FirstName {
            get => _dbUser.first_name;
            set => _dbUser.first_name = value;
        }

        public string LastName {
            get => _dbUser.last_name;
            set => _dbUser.last_name = value;
        }

        public string MiddleName {
            get => _dbUser.middle_name;
            set => _dbUser.middle_name = value;
        }

        [Required]
        [FioAttribue(ErrorMessage = "Invalid FIO")]
        public string Fio {
            get => LastName
                + (FirstName != null ? $" {FirstName}"  : "")
                + (MiddleName != null ? $" {MiddleName}" : "");
            
            set {
                var fio = value.Split(' ');
                LastName = fio.Length >= 1 ? fio[0] : null;
                FirstName = fio.Length >= 2 ? fio[1] : null;
                MiddleName = fio.Length >= 3 ? fio[2] : null;
            }
        }

        readonly Regex phoneRegex = new Regex(@"(?<=\+7|7|8)(\d{1,10})", RegexOptions.Compiled|RegexOptions.Singleline);
        [Required]
        [RussianPhone(ErrorMessage = "Invalid Phone")]
        public string Phone {
            get => "+7" + _dbUser.phone;
            set  {
                var m = phoneRegex.Match(value);
                _dbUser.phone = m.Value;
                OnPropertyChanged();
            }
        }
    }
    class PasswordConfirmUserData : UserData
    {
    }
}
