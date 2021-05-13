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

namespace kurs
{
    /// <summary>
    /// Interaction logic for CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public cart Cart { get; }
        public ObservableCollection<cart_items> Items => Cart.cart_items;
        public CartPage()
        {
            InitializeComponent();
            var user = Manager.CurrentUser;
            Cart = user?.CurrentOrderGetOrCreate()?.cart;
            CartLV.ItemsSource = Items;
            BottomStackpanel.DataContext = Cart;
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
    }
}
