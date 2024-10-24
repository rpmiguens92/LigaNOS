using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Models
{
     public class EmployeeViewModel : Employee
        {
            [Display(Name = "Image")]
            public IFormFile ImageFile { get; set; }

            [Display(Name = "Roles")]
            public string RoleId { get; set; }
            public IEnumerable<SelectListItem> Roles { get; set; }
        }
      
}
