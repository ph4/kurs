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
using System.Data.Entity.Validation;

namespace kurs
{

    /// <summary>
    /// Login/Registration ViewModel
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        public UserData UserData{ get; } = new UserData();

        public string ErrorText { get; private set; }
        public Visibility ErrorTextVisibility => ErrorText != null ? Visibility.Visible : Visibility.Collapsed;

        public bool Registration { get; private set; }
        public Visibility RegistrationVisibility => Registration ? Visibility.Visible : Visibility.Collapsed;

        public string ActionButtonContent => Registration ? "Register" : "Login";
        public string ChangeActionButtonContent => Registration ? "Back to login" : "To Registration";

        public LoginViewModel() : base()
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
            UserData.ValidateModel();
            var cred = ctx.credentials.FirstOrDefault(c => c.login.ToLower() == UserData.Login.ToLower());
            if (Registration)
            {
                if (cred != null)
                {
                    ErrorText = "User with this login exists";
                }
                else
                {
                    if (!UserData.HasErrors)
                    {
                        //throw new NotImplementedException();
                        cred = new credentials
                        {
                            password = UserData.Password.ToString(),
                            login = UserData.Login,
                            user_type = "user",
                        };
                        var fio = UserData.Fio.Split(' ');
                        var middle_name = fio.Length == 3 ? fio[2] : null;
                        var user = new user
                        {
                            phone = UserData.Phone,
                            last_name = fio[0],
                            first_name = fio[1],
                            middle_name = middle_name,
                            dob = null,
                            credentials = cred.id,
                        };
                        ctx.credentials.Add(cred);
                        ctx.user.Add(user);
                        try
                        {
                            ctx.SaveChanges();
                            Manager.CurrentUser = user;
                        } catch (DbEntityValidationException e)
                        {
                            ErrorText = e.EntityValidationErrors
                                .SelectMany(r => r.ValidationErrors)
                                .Aggregate(new StringBuilder(), (s, v) => s.AppendLine(v.ErrorMessage))
                                .ToString();
                        } catch (Exception e)
                        {
                            ErrorText = $"{e.GetType()}: \nFrom: {e.Source}\nMessage: {e.Message}";
                        } finally
                        {
                            ctx.user.Local.Clear();
                            ctx.credentials.Local.Clear();
                        }
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
                    if (!UserData.GetErrorsString("Password").Any() && !UserData.GetErrorsString("Login").Any())
                    {
                        if (cred.password == UserData.Password.ToString())
                        {
                            var user = cred.user.FirstOrDefault();
                            if (user == null && cred.user_type == "admin")
                            {
                                user = new user
                                {
                                    first_name = "SUPER",
                                    last_name = "ADMIN",
                                    phone = "9997777777",
                                    credentials = cred.id,
                                    credentials1 = cred,
                                };
                            }
                            Manager.CurrentUser = user;
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
