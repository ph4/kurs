using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace kurs
{
    public partial class cart_items : INotifyPropertyChanged
    {
        public string Name => product.name;

        public int Amount {
            get => amount;
            set => amount = value;
        }

        public decimal? PriceOriginal => product.Price;
        public decimal? PriceDiscount => product.PriceDiscount;

        public decimal? SumOriginal => PriceOriginal * amount;
        public decimal? SumDiscount => PriceDiscount * amount;


        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
