using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace kurs
{
    public partial class cart : INotifyPropertyChanged
    {
        private BindingList<cart_items> _items;
        public BindingList<cart_items> Items {
            get {
                if (_items is null)
                {
                    _items = new BindingList<cart_items>(cart_items);
                    _items.ListChanged += (s, e) => OnPropertyChangedMulti(new string[] { "TotalOriginal", "Total" });
                }
                return _items;
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnPropertyChangedMulti(IEnumerable<string> propertyNames)
        {
            foreach (var propertyName in propertyNames)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
