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
            var sql = "INSERT INTO dbo.[Contact] ([Name] ,[Email] ,[Phone] ,[Address] ,[PictureName], [SIN_Number]) VALUES" +
                "(@Name, @Email, @Phone, @Address, @Picturename, @SIN_Number); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT)";

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@Name", contact.Name, DbType.String, ParameterDirection.Input);
            parameter.Add("@Email", contact.Email, DbType.String, ParameterDirection.Input);
            parameter.Add("@Phone", contact.Phone, DbType.String, ParameterDirection.Input);
            parameter.Add("@Address", contact.Address, DbType.String, ParameterDirection.Input);
            parameter.Add("@Picturename", contact.PictureName, DbType.String, ParameterDirection.Input);
            parameter.Add("@SIN_Number", contact.SIN_Number, DbType.String, ParameterDirection.Input, 9);

            var id = this.db.Query<int>(sql, parameter).Single();

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