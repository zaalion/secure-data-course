using MyAddressBookPlus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAddressBookPlus
{
    public class ContactService
    {
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

        public int AddContact(Contact contact)
        {
            var context = new MyAddressBookPlusEntities();
            context.Contacts.Add(contact);
            context.SaveChanges();

            return contact.Id;
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