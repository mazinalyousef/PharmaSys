using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class UpdateUserDTO
    {
       [Required(ErrorMessage = "User Name is required")]

        #nullable enable
        public string? Username { get; set; }

         [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        

        public int ? DepartmentId{get;set;} 
    }
}