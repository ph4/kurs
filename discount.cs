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
    
    public partial class discount
    {
        public int id { get; set; }
        public int amount { get; set; }
        public int category_id { get; set; }
    
        public virtual category category { get; set; }
    }
}
