using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalIdentity.Resources.Tokens
{
    public class RevokeTokenResource
    {
        [Required]
        public string Token { get; set; }
    }
}
