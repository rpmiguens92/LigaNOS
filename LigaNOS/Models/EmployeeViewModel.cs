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
     
        public Guid ImageFileId { get; set; }

        public IFormFile ImageFile { get; set; }
 
        public IEnumerable<SelectListItem> Roles { get; set; }  
        public IEnumerable<SelectListItem> Clubs { get; set; }
    }

}
