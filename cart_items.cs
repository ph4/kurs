//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kurs
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class cart_items
    {
        public int id { get; set; }
        public int cart_id { get; set; }
        public int product_id { get; set; }
        public int amount { get; set; }
    
        public virtual cart cart { get; set; }
        public virtual product product { get; set; }
    }
}