//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DLMS.BO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Outlet
    {
        public Outlet()
        {
            this.Customers = new HashSet<Customer>();
            this.FeeCollections = new HashSet<FeeCollection>();
        }
    
        public int OutletID { get; set; }
        public string OutletCode { get; set; }
        public string OutletName { get; set; }
    
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<FeeCollection> FeeCollections { get; set; }
    }
}
