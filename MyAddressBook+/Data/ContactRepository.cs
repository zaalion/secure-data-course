using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MyAddressBookPlus.Models;

namespace MyAddressBookPlus.Data
{
    public class ContactRepository : IContactRepository
    {
        private IDbConnection db;

        public ContactRepository()
        {
            var connectionstring = ConfigurationManager.ConnectionStrings["SqlDataConnection"].ConnectionString;
            db = new SqlConnection(connectionstring);
        }

        public int AddContact(Contact contact)
        {
            var sql = "INSERT INTO dbo.[Contact] ([Name] ,[Email] ,[Phone] ,[Address] ,[PictureName]) VALUES" +
                "(@Name, @Email, @Phone, @Address, @Picturename); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var id = this.db.Query<int>(sql, contact).Single();
            contact.Id = id;
            return id;
        }

        public bool DeleteContact(int id)
        {
            var sql = "DELETE FROM dbo.[Contact] WHERE id = @id";
            var result = db.Execute(sql, new { Id = id });

            return true;
        }

        public Contact GetContact(int id)
        {
            var sql = "SELECT * FROM dbo.[Contact] WHERE id = @id";
            var result = db.Query<Contact>(sql, new { Id = id })
                .SingleOrDefault();

            return result;
        }

        public List<Contact> GetContacts()
        {
            var sql = "SELECT * FROM dbo.[Contact] order by id";
            var result = db.Query<Contact>(sql).ToList();

            return result;
        }
    }
}