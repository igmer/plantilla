using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalIdentity.Entities.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Usuario es requerido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email es requerido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
