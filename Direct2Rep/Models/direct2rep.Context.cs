﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Direct2Rep.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Direct2RepEntities : DbContext
    {
        public Direct2RepEntities()
            : base("name=Direct2RepEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Company_SaleRep> Company_SaleRep { get; set; }
        public virtual DbSet<RepStatePart> RepStateParts { get; set; }
        public virtual DbSet<SaleRepresentative> SaleRepresentatives { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Visitor> Visitors { get; set; }
        public virtual DbSet<Visitor_States> Visitor_States { get; set; }
    }
}
