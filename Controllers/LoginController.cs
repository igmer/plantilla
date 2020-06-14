using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using UniversalIdentity.Models;
using UniversalIdentity.Services;
using UniversalIdentity.Security.Tokens;
using Microsoft.AspNetCore.Identity;
using UniversalIdentity.Services.Communication;
using UniversalIdentity.Resources.Tokens;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversalIdentity.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IMapper mapper, IAuthenticationService authenticationService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [Route("/api/login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCredentials userCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(userCredentials.UserName, userCredentials.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Invalid credentials." });
            }

            var user = await _userManager.FindByNameAsync(userCredentials.UserName);
            var roles = await _userManager.GetRolesAsync(user);

            var response = _authenticationService.CreateAccessToken(user, roles);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            var accessTokenResource = _mapper.Map<AccessToken, AccessTokenResource>(response.Token);
            return Ok(accessTokenResource);
        }

        [AllowAnonymous]
        [Route("/api/token/refresh")]
        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenResource refreshTokenResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(refreshTokenResource.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid refresh token." });
            }
            var roles = await _userManager.GetRolesAsync(user);

            var response = _authenticationService.RefreshToken(refreshTokenResource.Token, user, roles);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            var tokenResource = _mapper.Map<AccessToken, AccessTokenResource>(response.Token);
            return Ok(tokenResource);
        }
        
        [Route("/api/token/revoke")]
        [HttpPost]
        public IActionResult RevokeToken([FromBody] RevokeTokenResource revokeTokenResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _authenticationService.RevokeRefreshToken(revokeTokenResource.Token);
            return NoContent();
        }
    }
}
