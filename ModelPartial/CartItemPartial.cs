using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs
{
    public partial class cart_items
    {
        public string Name => product.name;

        public decimal? PriceOriginal => product.price;
        public decimal? PriceDiscount => PriceOriginal - PriceOriginal * Discount;

        public decimal? SumOriginal => PriceOriginal * amount;
        public decimal? SumDiscount => PriceDiscount * amount;

        public decimal Discount => 
            (product.category.discount.Select(d => d.amount)
                .DefaultIfEmpty(0)
                .Max()
            ) / 100M;
    }
}
