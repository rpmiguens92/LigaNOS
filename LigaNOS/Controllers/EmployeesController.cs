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

namespace LigaNOS.Controllers
{
   
        public class EmployeesController : Controller
        {
            private readonly DataContext _context;
            private readonly IEmployeeRepository _employeeRepository;
            private readonly IUserHelper _userHelper;
            private readonly IConverterHelper _converterHelper;
            private readonly IBlobHelper _blobHelper;

            // GET: EmployeeController
            public IActionResult Index()
            {
                return View(_employeeRepository.GetAll().OrderBy(e => e.Name));
            }

            // GET: Employees/Details/5
            [Route("detailsemployee")]
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound(); // Substituindo por NotFound() padrão
                }

                var employee = await _employeeRepository.GetByIdAsync(id.Value);
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
                return View(model);
            }

            // POST: Employees/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("createemployee")]
            public async Task<IActionResult> Create(EmployeeViewModel model)
            {
                if (ModelState.IsValid)
                {
                    await _employeeRepository.AddRoleToEmployeeAsync(model, this.User.Identity.Name);
                    return RedirectToAction(nameof(Index));
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

                var model = new EmployeeViewModel
                {
                    Roles = _employeeRepository.GetComboRoles(),
                };

                ViewBag.Roles = model.Roles;
                return View(model);
            }

            // POST: Employees/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("editemployee")]
            public async Task<IActionResult> Edit(EmployeeViewModel model)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Guid imageId = Guid.Empty;

                        if (model.ImageFile != null && model.ImageFile.Length > 0)
                        {
                            imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees");
                        }

                        var employee = _converterHelper.ToEmployee(model, imageId, false);
                        employee.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                        employee.Role = model.RoleId;
                        await _employeeRepository.UpdateAsync(employee);
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

                    return RedirectToAction(nameof(Index));
                }

                return View(model);
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