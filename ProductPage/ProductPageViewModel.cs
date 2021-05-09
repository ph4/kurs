using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs.ProductStuff
{
    class ProductPageViewModel : ViewModelBase
    {
        public product Product { get; set; }
        public ProductPageViewModel() { }
        public ProductPageViewModel(product p)
        {
            Product = p;
        }
        public string Name =>  Product?.name;
        public decimal? Price => Product?.price;
        public string Json =>  Product?.specifications_json;
        public byte[] Image => Product?.image;
        public string Description => Product?.description;
        public ICollection<ISpecification> Specifications => Product?.Specifications;
    }
}
