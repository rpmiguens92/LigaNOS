using LigaNOS.Data;
using LigaNOS.Data.Repositories;
using LigaNOS.Helpers;
using LigaNOS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using LigaNOS.Data.Entities;
using System.Collections.Generic;

namespace LigaNOS.Controllers
{
   
public class EmployeesController : Controller
    {
        private readonly DataContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IUserRepository _userRepository;
        private readonly IMailHelper _mailHelper;
        RoleManager<IdentityRole> _roleManager;

        public EmployeesController(
            DataContext context,
            IUserRepository userRepository, 
            IEmployeeRepository employeeRepository, 
            IConverterHelper converterHelper,
            IBlobHelper blobHelper,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            RoleManager<IdentityRole> roleManager)

        {
            _context = context;
            _userRepository = userRepository;
            _blobHelper = blobHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _employeeRepository = employeeRepository;
            _mailHelper = mailHelper;
            _roleManager = roleManager;
        }

        // GET: EmployeeController
        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var employees = _employeeRepository.GetAllWithUsers()?.ToList();

            if (employees == null)
            {
                 ModelState.AddModelError(string.Empty, "No employees found.");
                return View(new List<Employee>()); // Retorna uma lista vazia
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                              e.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                              e.Role.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                                  .ToList();
            }

        
            return View(employees);
           // return View(_employeeRepository.GetAll().OrderBy(e => e.Name));
        }

        public IActionResult Search(string searchString)
        {
            var employees = _employeeRepository.GetAllWithUsers().ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e =>
                        e.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        e.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        e.Role.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return PartialView("_EmployeeTableBody", employees.ToList());  
        }

        // GET: Employees/Details/5
        [Route("detailsemployee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }

            //var employee = await _employeeRepository.GetByIdAsync(id.Value);
            var employee = await _employeeRepository.GetAllWithUsers()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Route("createemployee")]
        public IActionResult Create()
        {
            var model = new EmployeeViewModel
            {
                Roles = _employeeRepository.GetComboRoles(),
            };
            ViewBag.Roles = model.Roles;
            ViewBag.Clubs = new SelectList(_context.Clubs.ToList(), "Id", "Name");
            return View(model);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("createemployee")]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            //if (ModelState.IsValid)
            //{

            //    ViewBag.Roles = _employeeRepository.GetComboRoles();
            //    ViewBag.Clubs = new SelectList(_context.Clubs, "Id", "Name"); // Recarregar clubes
            //    return View(model);

            //}
            //await _employeeRepository.AddRoleToEmployeeAsync(model, this.User.Identity.Name);
            //await _employeeRepository.AddClubToEmployeeAsync(model.Id, model.ClubId);
            //return RedirectToAction(nameof(Index));
            if (!await _context.Clubs.AnyAsync(c => c.Id == model.ClubId))
            {
                ModelState.AddModelError("ClubId", "The selected club is not valid.");
            }

            if (!await _context.Roles.AnyAsync(r => r.Id == model.RoleId))
            {
                ModelState.AddModelError("RoleId", "The selected role is not valid.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _employeeRepository.GetComboRoles();
                ViewBag.Clubs = new SelectList(_context.Clubs, "Id", "Name");
                return View(model);
            }

            var clubExists = await _context.Clubs.AnyAsync(c => c.Id == model.ClubId);
            if (!clubExists)
            {
                ModelState.AddModelError("ClubId", "O clube selecionado não é válido.");
                ViewBag.Roles = _employeeRepository.GetComboRoles();
                ViewBag.Clubs = new SelectList(_context.Clubs, "Id", "Name");
                return View(model);
            }
           
            var employee = new Employee
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                RoleId = model.RoleId,
                ClubId = model.ClubId,
                ImageFileId = model.ImageFile != null
                              ? await _blobHelper.UploadBlobAsync(model.ImageFile, "employees")
                              : Guid.Empty,
            };


            await _employeeRepository.UpdateAsync(employee);

            var user = new User
            {
                FirstName = model.Name,
                LastName = model.Name,  
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userHelper.AddUserAsync(user, "123456"); 
            if (result.Succeeded)
            {
            
                await _userHelper.AddUserToRoleAsync(user, "Employee");
  
                var resetToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                 
                var resetLink = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = resetToken }, protocol: HttpContext.Request.Scheme);

                // Envia email  
                var response = _mailHelper.SendEmail(
                    user.Email,
                    "Welcome to LigaNOS - Account Confirmation",
                    $"<h1>Welcome! </h1><p>Create your password bu clicking on link below:</p><a href=\"{resetLink}\">Create Password</a>");

                if (!response.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {response.Message}");
                    return View(model);
                }

                ViewBag.Message = "Employee creation succeeded. It was sent an e-mail for account confirmation.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: Employees/Edit/5
        [Route("editemployee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            var clubs = await _context.Clubs.ToListAsync();

            var model = new EmployeeViewModel
            {
              
                Id = employee.Id,
                Name = employee.Name,
                Address = employee.Address,
                Phone = employee.Phone,
                Email = employee.Email,
                RoleId = employee.RoleId,
                ClubId = employee.ClubId,
                ImageFileId = employee.ImageFileId,
         
                Roles = _employeeRepository.GetComboRoles(),
                Clubs = new SelectList(clubs, "Id", "Name", employee.ClubId)
            };

            //ViewBag.Roles = model.Roles;
           // ViewBag.Clubs = new SelectList(await _context.Clubs.ToListAsync(), "Id", "Name", employee.Club);
            return View(model);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editemployee")]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
        {

             
            if (id != model.Id)
            {
                return NotFound();
            }

             
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            
            if (!ModelState.IsValid)
            {
               
                model.Roles = _employeeRepository.GetComboRoles();
                return View(model);
            }

            try
            {
                
                var employee = await _employeeRepository.GetByIdAsync(model.Id);
                if (employee == null)
                {
                    return NotFound();
                }

                // Upload  image 
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    employee.ImageFileId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees");
                }
                else
                {
                    employee.ImageFileId = model.ImageFileId;
                }

                
                employee.Name = model.Name;
                employee.Address = model.Address;
                employee.Phone = model.Phone;
                employee.Email = model.Email;
                employee.RoleId = model.RoleId;
                employee.ClubId = model.ClubId;

                
                employee.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

               
                await _employeeRepository.UpdateAsync(employee);

                ViewBag.Clubs = new SelectList(await _context.Clubs.ToListAsync(), "Id", "Name", employee.Club);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _employeeRepository.ExistAsync(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // GET: Employees/Delete/5
        [Route("deleteemployee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("deleteemployee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            try
            {
                await _employeeRepository.DeleteAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{employee.Name} is probably in use!";
                    ViewBag.ErrorMessage = $"{employee.Name} cannot be deleted!";
                }
            }

            return View("Error");
        }

        public IActionResult EmployeeNotFound()
        {
            return View();
        }
    }
    }