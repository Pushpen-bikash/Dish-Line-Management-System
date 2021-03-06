﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DLMSEntities : DbContext
    {
        public DLMSEntities()
            : base("name=DLMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Designaiton> Designaitons { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Expenditure> Expenditures { get; set; }
        public virtual DbSet<ExpenseItem> ExpenseItems { get; set; }
        public virtual DbSet<FeeCollDetail> FeeCollDetails { get; set; }
        public virtual DbSet<FeeCollection> FeeCollections { get; set; }
        public virtual DbSet<Outlet> Outlets { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<SystemInformation> SystemInformations { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<pcdExIEpen_Result> pcdExIEpen(Nullable<System.DateTime> entryDate)
        {
            var entryDateParameter = entryDate.HasValue ?
                new ObjectParameter("EntryDate", entryDate) :
                new ObjectParameter("EntryDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<pcdExIEpen_Result>("pcdExIEpen", entryDateParameter);
        }
    }
}
