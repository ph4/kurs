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

namespace kurs
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            if (Manager.CurrentUser == null)
                ToLogin();
            else OnUserChange(Manager.CurrentUser);
        }

        private void ToLogin()
        {
            MasterGrid.Children.Clear();
            Manager.UserChangeEvent += new Manager.UserChangeHandler(OnUserChange);
            MasterGrid.Children.Add(new LoginControl());
        }

        void OnUserChange(UserData user)
        {
            MasterGrid.Children.Clear();
            var tb = new TextBlock
            {
                Text = $"Logged in as {user.Login}\n FIO: {user.Fio}\n PHONE: {user.Phone}"
            };
            MasterGrid.Children.Add(tb);
        }
    }
}
