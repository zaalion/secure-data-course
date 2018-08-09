using MyAddressBookPlus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using System.Configuration;
using Newtonsoft.Json;
using MyAddressBookPlus.Models;

namespace MyAddressBookPlus.Services
{
    public class ContactService
    {
        // Redis cache initialization
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
            return ConnectionMultiplexer.Connect(cacheConnection);
        });        
        IDatabase cache = lazyConnection.Value.GetDatabase();

        /// <summary>
        /// Gets all contacts from database
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetContacts()
        {
            var contactRepository = new ContactRepository();
            var contacts = contactRepository.GetContacts();
            return contacts;
        }

        /// <summary>
        /// Gets a specific contact from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contact GetContact(int id)
        {
            var contactRepository = new ContactRepository();
            var contact = contactRepository.GetContact(id);

            return contact;
        }

        /// <summary>
        /// Gets a specific contact from Redis cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a new contact to the database and cache
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public int AddContact(Contact contact)
        {
            var contactRepository = new ContactRepository();
            contactRepository.AddContact(contact);

            var newId = contact.Id;

            // add new contact to cache
            cache.StringSet(newId.ToString(), JsonConvert.SerializeObject(contact));

            return newId;
        }

        /// <summary>
        /// deletes a given contact from database and cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteContact(int id)
        {
            var contactRepository = new ContactRepository();
            var success = contactRepository.DeleteContact(id);

            if (success)
            {
                // remove the item from cache
                cache.KeyDelete(id.ToString());
            }

            return success;
        }
    }
}