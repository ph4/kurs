using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            _currentProduct = selectedProduct ?? new product();

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

        private void BtnUploadImg_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image files (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (dlg.ShowDialog() ?? false)
            {
                _currentProduct.image = File.ReadAllBytes(dlg.FileName);

                //TODO
                BindingOperations.GetBindingExpression(ProductImage, Image.SourceProperty).UpdateTarget();
            }
        }
    }
}
