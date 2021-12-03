using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace kurs.ProductStuff
{
    class ProductPageViewModel : ViewModelBase
    {
        public product Product { get; set; }
        public ProductPageViewModel() {
            AddToCart = new RelayCommand(
                canExecute: o =>
                    Manager.CurrentUser != null,
                execute: o =>
                {
                    var ci = Manager.CurrentUser.CurrentOrderGetOrCreate().cart1.cart_items;
                    var item = ci.FirstOrDefault((i) => i.product_id == Product.id);

                    if (item != null) item.amount += 1;
                    else ci.Add(new cart_items { product = Product, amount = 1 });

                    Manager.SaveOrShowErrorMessage();

                    Manager.MainFrame.Navigate(new CartPage());
                }
            );
        }
        public ProductPageViewModel(product p) : this()
        {
            Product = p;
        }
        public string Name =>  Product?.name;
        public decimal? Price => Product?.Price;
        public decimal? PriceDiscount => Product?.PriceDiscount;
        public string Json => Product?.specifications_json;
        public byte[] Image => Product?.image;
        public string Description => Product?.description;
        public ICollection<ISpecification> Specifications => Product?.Specifications;

        public ICommand AddToCart { get; }
    }
}
