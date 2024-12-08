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
                var body = $@"
            <h1>New Contact Message</h1>
            <p><strong>Name:</strong> {model.Name}</p>
            <p><strong>Email:</strong> {model.Email}</p>
            <p><strong>Subject:</strong> {model.Subject}</p>
            <p><strong>Message:</strong></p>
            <p>{model.Message}</p>";


                Response response = _mailHelper.SendEmail("ritapereiramiguens@gmail.com", model.Subject, body);

            
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
                _flashMessage.Confirmation("Message sent! We apreciate your contact.");
            }
            return View("Index", model);
        }
    }
}
