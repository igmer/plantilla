using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalIdentity.Resources.Account
{
    public class UserModel
    {
        [Required(ErrorMessage = "Id es requerido")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Usuario es requerido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email es requerido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
