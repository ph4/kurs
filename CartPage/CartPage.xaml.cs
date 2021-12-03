using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;

namespace kurs
{
    /// <summary>
    /// Interaction logic for CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public cart Cart { get; }
        public address SelectedAddress { get; set; }

        //Enumerable.Append availale in .net versions 4.7.1 and above
        public IEnumerable<address> Addresses => (Cart?.order1.user.address ?? new ObservableCollection<address>())
            .Concat(new List<address>(1) { new address { address1 = "*New Address*" } });
        public CartPage()
        {
            var user = Manager.CurrentUser;
            if (user is null)
            {
                _ = MessageBox.Show("You need to login in order to access cart");
                return;
            }
            Cart = user.CurrentOrderGetOrCreate().cart1;
            InitializeComponent();
            CartLV.ItemsSource = Cart?.Items;
            //BottomStackpanel.DataContext = Cart;
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow row)
                {
                    Dns2Entities.GetContext().cart_items.Remove((cart_items)row.Item);
                    Manager.SaveOrShowErrorMessage();
                    break;
                }
        }

        private void BtnProceeed_Click (object sender, RoutedEventArgs e)
        {
            if (SelectedAddress is null) return;
            //New item
            var addressModal = new AddEditAddressWindow(SelectedAddress);
            var proceed = addressModal.ShowDialog() ?? false;
            if (proceed)
            {
                Manager.MainFrame.Navigate(new ReceitPage());
            }
        }
    }
}
