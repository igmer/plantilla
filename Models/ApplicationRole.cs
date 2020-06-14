using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalIdentity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        //public ApplicationRole(string roleName, string description) : base(roleName)
        //{
        //    base.Name = roleName;
        //    Description = description;            
        //}
    }
}
