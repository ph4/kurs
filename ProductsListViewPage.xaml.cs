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
using System.Data.Entity;

namespace kurs
{
    /// <summary>
    /// Interaction logic for ProductsListViewPage.xaml
    /// </summary>
    public partial class ProductsListViewPage : Page
    {
        private Dns2Entities ctx;
        public ProductsListViewPage()
        {
            InitializeComponent();
            ctx = Dns2Entities.GetContext();

            var categories = ctx.category.ToList();
            categories.Insert(0, new category
                {
                    name = "All categories",
                }
            );
            ComboCategory.ItemsSource = categories;
            ComboCategory.SelectedIndex = 0;

            UpdateProducts();
        }

        private void UpdateProducts()
        {
            var currentProducts = ctx.product.ToList().AsQueryable();
            if (ComboCategory.SelectedIndex > 0)
            {
                currentProducts = currentProducts.Where(p => p.category.id == (ComboCategory.SelectedItem as category).id);
            }

            if (!string.IsNullOrWhiteSpace(TBoxSearch.Text))
                currentProducts = currentProducts.Where(p => p.name.ToLower().Contains(TBoxSearch.Text.ToLower()));

            LViewProducts.ItemsSource = currentProducts.OrderBy(p => p.name.ToLower()).ToList();
        }

        private void ComboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            var card = (MaterialDesignThemes.Wpf.Card)sender;
        }

        private void LViewProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var product = (product)LViewProducts.SelectedItem;
            LViewProducts.SelectedItem = null;
            Manager.MainFrame.Navigate(new ProductStuff.ProductPage(product));
        }
    }
}
