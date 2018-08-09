using MyAddressBookPlus.Models;
using MyAddressBookPlus.Services;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MyAddressBookPlus.Controllers
{
    public class ContactController : Controller
    {
        /// <summary>
        /// Default action which shows a list of all contacts
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var contactService = new ContactService();

            var contacts = contactService.GetContacts();

            var viewModel = contacts.Select(c => new Contact()
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address
            });

            return View(viewModel);
        }

        /// <summary>
        /// Shows "Add" form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Adds a new contact to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(Contact model)
        {
            var contactService = new ContactService();

            // Handling file upload; save the uploaded contact picture into Azure blob storage.
            string pictureFilename = string.Empty;
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

            var id = contactService.AddContact(new Contact()
            {
                Name = model.Name,
                Address = model.Address,
                Email = model.Email,
                Phone = model.Phone,
                PictureName = pictureFilename
            });

            return RedirectToAction("index");
        }

        /// <summary>
        /// Shows specific contact details from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var contactService = new ContactService();
            var contact = contactService.GetContact(id);

            var photoContainerUrl = ConfigurationManager.AppSettings["photoContainerUrl"];

            return View(new Contact()
            {
                Id = contact.Id,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                PictureName = string.IsNullOrEmpty(contact.PictureName) ? null : $"{photoContainerUrl}{contact.PictureName}"
            });
        }

        /// <summary>
        /// Shows specific contact details from Redis cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailsCache(int id)
        {
            var contactService = new ContactService();
            var contact = contactService.GetContactFromCache(id);

            // in case the key does not exist in the cache; returning a fall-back model
            if(contact == null)
            {
                return View(new Contact()
                {
                    Id = -1,
                    Name = "Cache is not available",
                    Phone = "Null",
                    Email = "Null",
                    Address = "Null",
                    PictureName = null
                });
            }

            var photoContainerUrl = ConfigurationManager.AppSettings["photoContainerUrl"];

            return View(new Contact()
            {
                Id = contact.Id,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                PictureName = string.IsNullOrEmpty(contact.PictureName) ? null : $"{photoContainerUrl}{contact.PictureName}"
            });
        }

        /// <summary>
        /// Deletes a specific contact from database and cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var contactService = new ContactService();

            var success = contactService.DeleteContact(id);

            return RedirectToAction("index");
        }
    }
}