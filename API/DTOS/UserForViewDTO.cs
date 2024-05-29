using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class UserForViewDTO
    {
         
         public string Id { get; set; }
         public string UserName { get; set; }
         public string Email { get; set; }

         public string Department{get;set;}
    }
}