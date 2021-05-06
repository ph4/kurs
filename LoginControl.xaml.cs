using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections;

namespace kurs
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        private const int PASSWORD_MIN = 8;
        private LoginControlViewModel vm;

        public LoginControl()
        {
            InitializeComponent();
            vm = new LoginControlViewModel();
            DataContext = vm;
        }

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            vm.DoAction();
        }

        private void BtnChangeAction_Click(object sender, RoutedEventArgs e)
        {
            vm.ChangeType();
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var len = PasswordInput.Password.Length;
            if (len != 0 && len < 8)
            {
                MaterialDesignThemes.Wpf.HintAssist.SetHelperText(PasswordInput, "Minimum length is 8 symbols");
            } else
            {
                MaterialDesignThemes.Wpf.HintAssist.SetHelperText(PasswordInput, null);
            }
            vm.Password = ((PasswordBox)sender).Password;
        }

        private void PasswordConfirmInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            vm.PasswordConfirm = ((PasswordBox)sender).Password;
        }
    }
}
