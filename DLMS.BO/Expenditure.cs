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
    
    public partial class Expenditure
    {
        public int ExpenditureID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Purpose { get; set; }
        public decimal Amount { get; set; }
        public int ExpenseItemID { get; set; }
    
        public virtual ExpenseItem ExpenseItem { get; set; }
    }
}
