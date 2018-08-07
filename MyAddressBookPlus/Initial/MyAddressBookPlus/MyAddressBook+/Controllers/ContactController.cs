using MyAddressBookPlus.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(ContactViewModel model)
        {
            var contactService = new ContactService();

            string pictureFilename = string.Empty;

            // handling file upload
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);

                    var blobService = new BlobService();
                    pictureFilename = blobService.UploadPictureToBlob(Server.MapPath("~/Images/"), fileName);
                }
            }

            var id = contactService.AddContact(new Data.Contact()
            {
                Name = model.Name,
                Address = model.Address,
                Email = model.Email,
                Phone = model.Phone,
                PictureName = pictureFilename
            });

            return RedirectToAction("index");
        }

        public ActionResult Details(int id)
        {
            var contactService = new ContactService();
            var contact = contactService.GetContact(id);

            var photoContainerUrl = ConfigurationManager.AppSettings["photoContainerUrl"];

            return View(new ContactViewModel()
            {
                Id = contact.Id,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                PhotoUrl = string.IsNullOrEmpty(contact.PictureName) ? null : $"{photoContainerUrl}{contact.PictureName}"
            });
        }

        public ActionResult DetailsCache(int id)
        {
            var contactService = new ContactService();
            var contact = contactService.GetContactFromCache(id);

            // in case the key does not exist in the cache.
            if(contact == null)
            {
                return View(new ContactViewModel()
                {
                    Id = -1,
                    Name = "Cache is not available",
                    Phone = "Null",
                    Email = "Null",
                    Address = "Null",
                    PhotoUrl = null
                });
            }

            var photoContainerUrl = ConfigurationManager.AppSettings["photoContainerUrl"];

            return View(new ContactViewModel()
            {
                Id = contact.Id,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                PhotoUrl = string.IsNullOrEmpty(contact.PictureName) ? null : $"{photoContainerUrl}{contact.PictureName}"
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