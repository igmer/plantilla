
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniversalIdentity.Security.Tokens;

namespace UniversalIdentity.Models
{
    public class RefreshToken : JsonWebToken
    {       
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public RefreshToken(string userId, string token, long expiration) : base(token, expiration)
        {
            UserId = userId;
        }
    }
}