using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversalIdentity.Entities.Account;
using UniversalIdentity.Models;
using UniversalIdentity.Resources.Account;

namespace UniversalIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        
        private IMapper _mapper;       
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController( UserManager<ApplicationUser> userManager, IMapper mapper)        
        {            
            _mapper = mapper;           
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var response = await _userManager.CreateAsync(user, model.Password);
            if (!response.Succeeded)
            {
                return BadRequest();
            }
            return Ok();            
        }

       [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var models = new List<UserModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var (user, model) in from user in users let model = _mapper.Map<UserModel>(user) select (user, model))
            {
                model.Roles = await _userManager.GetRolesAsync(user);
                models.Add(model);
            }

            return Ok(models);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Id de usuario no puede ser un valor nulo" });
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<UserModel>(user);

            model.Roles = await _userManager.GetRolesAsync(user);
            return Ok(model);
        }

         [Authorize(AuthenticationSchemes = "Bearer")]
        /// <summary>
        /// Actualiza el nombre de usuario y el correo electrónico de la cuenta correspondiente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody]UpdateModel model)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Id de usuario no puede ser un valor nulo" });
            }

            if (id != model.Id)
            {
                return BadRequest(new { message = "Id de usuario no pasó verificación" });
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();            
        }

         [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return BadRequest(new { message = "Id de usuario no puede ser un valor nulo" });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = _userManager.DeleteAsync(user);
            if (!result.IsCompletedSuccessfully)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}