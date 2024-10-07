using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;


namespace LigaNOS.Data.Entities
{
    public class User : IdentityUser 
    { 
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
