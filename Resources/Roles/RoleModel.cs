using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalIdentity.Resources.Roles
{
    public class RoleModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        public string Name { get; set; }

       
        public string Description { get; set; }
    }
}
