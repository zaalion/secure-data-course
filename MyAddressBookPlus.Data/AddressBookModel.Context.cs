﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyAddressBookPlus.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using Microsoft.Azure.Services.AppAuthentication;
    using System.Web.Configuration;

    public partial class MyAddressBookPlusEntities : DbContext
    {
        public MyAddressBookPlusEntities(SqlConnection conn)
            : base(conn, true)
        {
            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["MyAddressBookPlusEntities"].ConnectionString;
            conn.AccessToken = (new AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
            Database.SetInitializer<MyAddressBookPlusEntities>(null);
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Contact> Contacts { get; set; }
    }
}
