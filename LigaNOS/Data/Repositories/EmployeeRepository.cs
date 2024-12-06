using LigaNOS.Data.Entities;
using LigaNOS.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using LigaNOS.Models;
using Microsoft.EntityFrameworkCore;

namespace LigaNOS.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public EmployeeRepository(DataContext context,
            IUserHelper userHelper,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        public async Task AddClubToEmployeeAsync(int id, int clubId)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            
            var club = await _context.Clubs.FindAsync(clubId);
            if (club == null)
            {
                throw new Exception("Club not found.");
            }

            
            employee.ClubId = clubId;

            // update
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task AddRoleToEmployeeAsync(EmployeeViewModel model, string userName)
        { 
           
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new ArgumentException("Email cannot be null or empty");
            }

        
            Guid imageId = Guid.Empty;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees");
            }

             
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

 
            var employee = new Employee
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                RoleId = model.RoleId,  
                ImageFileId = imageId,
                User = user,
            };
 
            _context.Employees.Add(employee);

 
            await _context.SaveChangesAsync();
        }

        public IQueryable<Employee> GetAllWithUsers()
        {
            return _context.Employees
                .Include(e => e.User)
                .Include(e => e.Role)
                .Include(e => e.Club);

        }

        public IEnumerable<SelectListItem> GetComboEmployess()
        {
            var list = _context.Employees.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select the Employee...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
        
            return _context.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();
        }
    }
}
