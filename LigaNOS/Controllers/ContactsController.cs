using LigaNOS.Data;
using LigaNOS.Data.Entities;
using LigaNOS.Helpers;
using LigaNOS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vereyon.Web;

namespace LigaNOS.Controllers
{
    public class ContactsController : Controller 
    { 

    private readonly IMailHelper _mailHelper;
    private readonly IFlashMessage _flashMessage;
    private readonly DataContext _context;
        public ContactsController(IMailHelper mailHelper,
            IFlashMessage flashMessage,
            DataContext context)
        {
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
            _context = context;
        }
        // GET: ContactsController
        public ActionResult Index()
        {

            return View(new ContactViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> SendMail(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                Response response = _mailHelper.SendEmail("ritapereiramiguens@gmail.com", model.Subject, model.Message);

            
                var contact = new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    Subject = model.Subject,
                    Message = model.Message
                };

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                if (response.IsSuccess)
                {
                    _flashMessage.Confirmation("Message sent!");
                    return RedirectToAction("Index");
                }
                else
                {
                    _flashMessage.Danger("Error! Message not sent.");
                }
            }
            return View("Index", model);
        }
    }
}
