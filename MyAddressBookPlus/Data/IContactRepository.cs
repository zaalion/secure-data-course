using System.Collections.Generic;
using MyAddressBookPlus.Models;

namespace MyAddressBookPlus.Data
{
    public interface IContactRepository
    {
        Contact GetContact(int id);

        int AddContact(Contact contact);

        bool DeleteContact(int id);

        List<Contact> GetContacts();
    }
}