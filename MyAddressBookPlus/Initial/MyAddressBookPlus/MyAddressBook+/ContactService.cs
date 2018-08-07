using MyAddressBookPlus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;
using System.Configuration;
using Newtonsoft.Json;

namespace MyAddressBookPlus
{
    public class ContactService
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        IDatabase cache = lazyConnection.Value.GetDatabase();

        public List<Contact> GetContacts()
        {
            var context = new MyAddressBookPlusEntities();
            var contacts = context.Contacts.ToList();
            return contacts;
        }

        public Contact GetContact(int id)
        {
            var context = new MyAddressBookPlusEntities();
            var contact = context.Contacts.SingleOrDefault(c => c.Id == id);

            return contact;
        }

        public Contact GetContactFromCache(int id)
        {
            var cacheContent = cache.StringGet(id.ToString());
            if (!cacheContent.IsNull)
            {
                var contact = JsonConvert.DeserializeObject<Contact>(cacheContent);
                return contact;
            }

            return null;
        }

        public int AddContact(Contact contact)
        {
            var context = new MyAddressBookPlusEntities();
            context.Contacts.Add(contact);
            context.SaveChanges();

            var newId = contact.Id;

            // add new contact to cache
            cache.StringSet(newId.ToString(), JsonConvert.SerializeObject(contact));

            return newId;
        }

        public bool DeleteContact(int id)
        {
            var context = new MyAddressBookPlusEntities();
            var contactToDelete = context.Contacts.SingleOrDefault(c => c.Id == id);

            if(contactToDelete == null)
            {
                return false;
            }

            context.Contacts.Remove(contactToDelete);
            context.SaveChanges();

            return true;
        }
    }
}