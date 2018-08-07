using MyAddressBookPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAddressBookPlus.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            var contactService = new ContactService();

            var contacts = contactService.GetContacts();

            var viewModel = contacts.Select(c => new ContactViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address
            });

            return View(viewModel);
        }

        public ActionResult Add(ContactViewModel model)
        {
            var contactService = new ContactService();

            var id = contactService.AddContact(new Data.Contact()
            {
                Name = model.Name,
                Address = model.Address,
                Email = model.Email,
                Phone = model.Phone
            });

            return RedirectToAction("index");
        }

        public ActionResult Details(int id)
        {
            var contactService = new ContactService();

            var contact = contactService.GetContact(id);

            return View(new ContactViewModel()
            {
                Id = contact.Id,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address
            });
        }

        public ActionResult Delete(int id)
        {
            var contactService = new ContactService();

            var success = contactService.DeleteContact(id);

            return RedirectToAction("index");
        }
    }
}