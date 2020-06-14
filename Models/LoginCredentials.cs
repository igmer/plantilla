using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalIdentity.Models
{
    public class LoginCredentials
    {
        [Required(ErrorMessage = "Usuario es requerido")]           
        public string UserName { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida")]
        public string Password { get; set; }
    }
}
