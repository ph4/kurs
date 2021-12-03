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
using System.Windows.Shapes;

namespace kurs
{
    /// <summary>
    /// Interaction logic for AddEditAddressWindow.xaml
    /// </summary>
    public partial class AddEditAddressWindow : Window
    {
        public address Adress { get; }
        public bool IsNew => Adress.id == 0;

        public AddEditAddressWindow(address adress)
        {
            Adress = adress;
            InitializeComponent();

            // Force alt-tab to focus this window when switching to MainWidow
            Owner = Application.Current.MainWindow;
        }

        private void Proceed_click(object sender, RoutedEventArgs e)
        {
            var ctx = Dns2Entities.GetContext();
            if (IsNew) ctx.address.Add(Adress);
            Manager.CurrentUser.CurrentOrderGetOrCreate().address = Adress;
            DialogResult = Manager.TrySaveOrShowErrorMessage();
            Close();
        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
