using LigaNOS.Data.Entities;
using LigaNOS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboEmployess();

        IEnumerable<SelectListItem> GetComboRoles();

        public Task AddRoleToEmployeeAsync(EmployeeViewModel model, string userName);
    }
}