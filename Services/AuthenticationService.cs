using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalIdentity.Models;
using UniversalIdentity.Security.Tokens;
using UniversalIdentity.Services.Communication;

namespace UniversalIdentity.Services
{
    public interface IAuthenticationService
    {
        TokenResponse CreateAccessToken(ApplicationUser user, IEnumerable<string> roles );
        TokenResponse RefreshToken(string refreshToken, ApplicationUser user, IEnumerable<string> roles);
        void RevokeRefreshToken(string refreshToken);
    }
    public class AuthenticationService : IAuthenticationService
    {
       
        private readonly ITokenProviderService _tokenProvider;

        public AuthenticationService(ITokenProviderService tokenProvider)
        {
            _tokenProvider = tokenProvider;
           
        }

        public TokenResponse CreateAccessToken(ApplicationUser user, IEnumerable<string> roles)
        {

            var token = _tokenProvider.CreateAccessToken(user, roles);

            return new TokenResponse(true, null, token);
        }

        public TokenResponse RefreshToken(string refreshToken, ApplicationUser user, IEnumerable<string> roles)
        {
            var token = _tokenProvider.TakeRefreshToken(refreshToken);

            if (token == null)
            {
                return new TokenResponse(false, "Invalid refresh token.", null);
            }

            if (token.IsExpired())
            {
                return new TokenResponse(false, "Expired refresh token.", null);
            }

            var accessToken = _tokenProvider.CreateAccessToken(user, roles);
            return new TokenResponse(true, null, accessToken);
        }

        public void RevokeRefreshToken(string refreshToken)
        {
            _tokenProvider.RevokeRefreshToken(refreshToken);
        }
    }
}
