using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using UniversalIdentity.Data;
using UniversalIdentity.Models;

namespace UniversalIdentity.Security.Tokens
{
    public interface ITokenProviderService
    {
        AccessToken CreateAccessToken(ApplicationUser user, IEnumerable<string> roles);
        RefreshToken TakeRefreshToken(string token);
        void RevokeRefreshToken(string token);
    }

    public class TokenProvider : ITokenProviderService
    {        
        private readonly TokenProviderOptions _tokenOptions;
        private readonly IPasswordHasher _passwordHaser;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly ApplicationDbContext _dbContext;
         public TokenProvider(ApplicationDbContext dbcontext, SigningConfigurations signingConfigurations, IPasswordHasher passwordHaser, IOptions<TokenProviderOptions> tokenOptionsSnapshot)
        {
           
            _tokenOptions = tokenOptionsSnapshot.Value;          
            _passwordHaser = passwordHaser;
            _signingConfigurations = signingConfigurations;
            _dbContext = dbcontext;
        }

        public AccessToken CreateAccessToken(ApplicationUser user, IEnumerable<string> roles)
        {
            var refreshToken = BuildRefreshToken(user);
            var accessToken = BuildAccessToken(user, roles, refreshToken);
            try
            {
                _dbContext.Add(refreshToken);
                _dbContext.SaveChanges();
            }catch(Exception ex)
            {
                return null;
            }
           

            return accessToken;
        }

        public RefreshToken TakeRefreshToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var refreshToken = _dbContext.RefreshTokens.SingleOrDefault(t => t.Token == token);
            if (refreshToken != null)
            {
                _dbContext.RefreshTokens.Remove(refreshToken);
                _dbContext.SaveChanges();
            }

            return refreshToken;
        }

        public void RevokeRefreshToken(string token)
        {
            TakeRefreshToken(token);
        }

        private RefreshToken BuildRefreshToken(ApplicationUser User)
        {            
            var refreshToken = new RefreshToken
            (      
                userId: User.Id,
                token: _passwordHaser.HashPassword(Guid.NewGuid().ToString()),
                expiration: DateTime.UtcNow.AddMinutes(_tokenOptions.RefreshTokenExpiration).Ticks
            );
           
            return refreshToken;
        }

        private AccessToken BuildAccessToken(ApplicationUser user, IEnumerable<string> roles, RefreshToken refreshToken)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);

            var securityToken = new JwtSecurityToken
            (
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: GetClaims(user, roles),
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: _signingConfigurations.SigningCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(securityToken);

            return new AccessToken(accessToken, accessTokenExpiration.Ticks, refreshToken);
        }

        private IEnumerable<Claim> GetClaims(ApplicationUser user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email)
            };

            roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));


            return claims;
        }
    }
}