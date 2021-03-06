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

namespace kurs.ProductStuff
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage(product p)
        {
            InitializeComponent();
            ((ProductPageViewModel)DataContext).Product = p;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Disable selection
            ((ListView)sender).SelectedItem = null;
        }
    }
}
