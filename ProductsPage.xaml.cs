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
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            DGridModel.ItemsSource = Dns2Entities.GetContext().product.ToList();
        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            var p = (sender as Button).DataContext as product;
            Manager.MainFrame.Navigate(new AddEditPage(p));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var forRemove = DGridModel.SelectedItems.Cast<product>().ToList();
            if ( MessageBox.Show($"Are you sure yo want to delete {forRemove.Count}", "Attention", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var ctx = Dns2Entities.GetContext();
                    ctx.product.RemoveRange(forRemove);
                    ctx.SaveChanges();
                    MessageBox.Show("Remove success");

                    DGridModel.ItemsSource = ctx.product.ToList();
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Dns2Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridModel.ItemsSource = Dns2Entities.GetContext().product.ToList();
            }
        }
    }
}
