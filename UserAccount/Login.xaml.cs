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
        private LoginViewModel Vm => (LoginViewModel)DataContext;

        public LoginControl()
        {
            InitializeComponent();
        }

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            Vm.DoAction();
        }

        private void BtnChangeAction_Click(object sender, RoutedEventArgs e)
        {
            Vm.ChangeType();
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Vm.UserData.Password = ((PasswordBox)sender).Password;
        }

        private void PasswordConfirmInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Vm.PasswordConfirm = ((PasswordBox)sender).Password;
        }
    }
}
