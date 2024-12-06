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
        RoleManager<IdentityRole> _roleManager;

        public EmployeesController(
            DataContext context,
            IUserRepository userRepository, 
            IEmployeeRepository employeeRepository, 
            IConverterHelper converterHelper,
            IBlobHelper blobHelper,
            IUserHelper userHelper,
            RoleManager<IdentityRole> roleManager)

        {
            _context = context;
            _userRepository = userRepository;
            _blobHelper = blobHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _employeeRepository = employeeRepository;
            _roleManager = roleManager;
        }

        // GET: EmployeeController
        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var employees = _employeeRepository.GetAllWithUsers().ToList();

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
                return NotFound(); // Substituindo por NotFound() padrão
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

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _employeeRepository.GetComboRoles();
                ViewBag.Clubs = new SelectList(_context.Clubs, "Id", "Name");
                return View(model);
            }

            // Cria o funcionário e salva no banco
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

            return RedirectToAction(nameof(Index));
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

            var model = new EmployeeViewModel
            {
                //Roles = _employeeRepository.GetComboRoles(),
                Id = employee.Id,
                Name = employee.Name,
                Address = employee.Address,
                Phone = employee.Phone,
                Email = employee.Email,
                RoleId = employee.RoleId,
                ImageFileId = employee.ImageFileId,
         
                Roles = _employeeRepository.GetComboRoles()
            };

            ViewBag.Roles = model.Roles;
            ViewBag.Clubs = new SelectList(await _context.Clubs.ToListAsync(), "Id", "Name", employee.Club);
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