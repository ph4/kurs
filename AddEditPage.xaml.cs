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
    /// Interaction logic for AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private product _currentProduct;
        public AddEditPage(product selectedProduct)
        {
            if (selectedProduct == null)
                _currentProduct = new product();
            else
                _currentProduct = selectedProduct;

            InitializeComponent();
            // _currentProduct = Dns2Entities.GetContext().
            DataContext = _currentProduct;
            comboCategory.ItemsSource = Dns2Entities.GetContext().category.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentProduct.name))
                errors.AppendLine("Name can't be empty");
            if (_currentProduct.category == null)
                errors.AppendLine("Category can't be empty");

            if (errors.Length > 0) {
                MessageBox.Show(errors.ToString(), "Invalid Product");
                return;
            }
            var ctx = Dns2Entities.GetContext();
            if (_currentProduct.id == 0)
            {
                ctx.product.Add(_currentProduct);
            }
            try
            {
                ctx.SaveChanges();
                MessageBox.Show("Save Succes");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error saving changes");
            }
        }
    }
}
