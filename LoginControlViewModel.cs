using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using kurs.Validation;

namespace kurs
{
    class FioAttribue : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var str = value.ToString();
            var fio = str.Trim().Split();
            return (fio.Length == 2 || fio.Length == 3) && fio.All(s => s.All(char.IsLetter));
        }
    }

    class RussianPhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var regex = new Regex(@"(\+7)|(8)?\d{10}");
            return regex.IsMatch(value.ToString());
        }
    }

    class LoginControlViewModel : ViewModelBase
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must me at least 8 chars")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords dont match")]
        public string PasswordConfirm { get; set; }

        [Required]
        [FioAttribue(ErrorMessage = "Invalid FIO")]
        public string Fio { get; set; }


        [Required]
        [RussianPhone(ErrorMessage = "Invalid Phone")]
        public string Phone { get; set; }

        public string ErrorText { get; private set; }
        public Visibility ErrorTextVisibility => ErrorText != null ? Visibility.Visible : Visibility.Collapsed;

        public bool Registration { get; private set; }
        public Visibility RegistrationVisibility => Registration ? Visibility.Visible : Visibility.Collapsed;

        public string ActionButtonContent => Registration ? "Register" : "Login";
        public string ChangeActionButtonContent => Registration ? "Back to login" : "To Registration";

        public LoginControlViewModel() : base()
        {
            Registration = false;
        }

        public void ChangeType()
        {
            Registration = !Registration;
            ErrorText = null;
        }

        public void DoAction()
        {
            var ctx = Dns2Entities.GetContext();
            ErrorText = null;
            Validate();
            var cred = ctx.credentials.FirstOrDefault(c => c.login.ToLower() == Login.ToLower());
            if (Registration)
            {
                if (cred != null)
                {
                    ErrorText = "User with this login exists";
                }
                else
                {
                    if (!this.HasErrors)
                    {
                        //throw new NotImplementedException();
                        cred = new credentials
                        {
                            password = Password.ToString(),
                            login = Login,
                            user_type = "user",
                        };
                        var fio = Fio.Split(' ');
                        var middle_name = fio.Length == 3 ? fio[2] : null;
                        var user = new user
                        {
                            phone = Phone,
                            last_name = fio[0],
                            first_name = fio[1],
                            middle_name = middle_name,
                            dob = null,
                            credentials = cred.id,
                        };
                        
                    }
                    else
                    {
                        ErrorText = "Check fields";
                    }
                }
            }
            else
            {
                if (cred == null)
                {
                    ErrorText = "Login not found";
                }
                else
                {
                    if (!GetErrorsString("Password").Any() && !GetErrorsString("Login").Any())
                    {
                        if (cred.password == Password.ToString())
                        {
                            var user = cred.user.FirstOrDefault();
                            if (user == null && cred.user_type == "admin")
                            {
                                user = new user
                                {
                                    first_name = "SUPER",
                                    last_name = "ADMIN",
                                    phone = "9997771122",
                                    credentials = cred.id,
                                };
                            }
                            Manager.CurrentUser = user;
                            Manager.NotifyUserChange(user);
                        }
                        else
                        {
                            ErrorText = "Incorrect password";
                        }
                    }
                    else ErrorText = "Check Input fields";
                }
            }
        }

    }
}
