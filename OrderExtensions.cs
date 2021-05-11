using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs
{
    public partial class order
    {
        //  CHECK ([order_status] IN ('checkout', 'payment', 'delivery', 'complete')) DEFAULT 'checkout'
        public enum OrderStatus
        {
            Unknown,
            Checkout,
            Payment,
            Delivery,
            Complete,
        }

        public OrderStatus Status {
            get => Enum.TryParse(order_status, true, out OrderStatus res) ? res : OrderStatus.Unknown;
            set => order_status = value.ToString().ToLower();
        }
    }
}
