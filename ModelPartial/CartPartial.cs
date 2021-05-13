using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs
{
    public partial class cart
    {
        public decimal TotalOriginal => cart_items.Select(ci => ci.SumOriginal)
            .Where(s => s != null)
            .Cast<decimal>()
            .DefaultIfEmpty(0)
            .Sum();

        public decimal Total => cart_items.Select(ci => ci.SumDiscount)
            .Where(s => s != null)
            .Cast<decimal>()
            .DefaultIfEmpty(0)
            .Sum();
    }
}
