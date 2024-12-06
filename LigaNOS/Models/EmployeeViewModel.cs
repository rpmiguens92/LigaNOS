using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Models
{
     public class EmployeeViewModel : Employee
        {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Guid ImageFileId { get; set; }

        public IFormFile ImageFile { get; set; }

        public string RoleId { get; set; }  

        public IEnumerable<SelectListItem> Roles { get; set; }  
        public IEnumerable<SelectListItem> Clubs { get; set; }
    }

}
