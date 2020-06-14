using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalIdentity.Models;

namespace UniversalIdentity.Data
{
    public static class DataInitializer
    {
        private const string adminuser = "admin@universalidentity.com";
        private const string adminpassword = "p@$$w0rD";
        private const string adminrole = "Admin";
        private const string adminroledescription = "Este es el rol de administrador";
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {

            if (userManager.FindByEmailAsync(adminuser).Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminuser,
                    Email = adminuser
                };                

                IdentityResult result = userManager.CreateAsync(user, adminpassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, adminuser).Wait();
                }
            }            
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {

            if (!roleManager.RoleExistsAsync(adminrole).Result)
            {
                var roleResult = roleManager.CreateAsync(new ApplicationRole { Name = adminrole, Description = adminroledescription}).Result;
            }
        }
    }
}
