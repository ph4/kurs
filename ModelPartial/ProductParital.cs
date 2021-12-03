using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs
{
    public partial class product
    {
        public decimal? Price => price;
        public decimal? PriceDiscount => Price - (Price * Discount);
        public decimal? Discount => category.discount.Select(d => d.amount as int?)
                .DefaultIfEmpty(null)
                .Max() / 100M ?? 0;
    }
}
