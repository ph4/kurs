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
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var regex = new Regex(@"(\+7)|(8)?\d{10}");
            return regex.IsMatch(value.ToString());
        }
    }

    public class UserData : AnnotationValidationViewModel
    {
        private readonly user dbUser;
        public UserData() { }
        public UserData(user dbUser)
        {
            this.dbUser = dbUser;
            var cred = dbUser.credentials1;
            Login = cred.login;
            Password = cred.password;

            var sb = new StringBuilder();
            sb.Append(dbUser.last_name).Append(' ').Append(dbUser.first_name);
            if (dbUser.middle_name != null)
                sb.Append(' ').Append(dbUser.middle_name);
            Fio = sb.ToString();

            Phone = dbUser.phone;
        }

        public void UpdateDb()
        {
            throw new NotImplementedException();
        }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must me at least 8 chars")]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password", ErrorMessage = "Passwords dont match")]
        public string PasswordConfirm { get; set; } = "";

        [Required]
        [FioAttribue(ErrorMessage = "Invalid FIO")]
        public string Fio { get; set; }

        [Required]
        [RussianPhone(ErrorMessage = "Invalid Phone")]
        public string Phone { get; set; }
    }
}
