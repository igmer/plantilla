using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversalIdentity.Data;
using UniversalIdentity.Models;
using UniversalIdentity.Resources.Roles;

namespace UniversalIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        public RolesController(IMapper mapper, ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
        }

       
        // GET: api/ApplicationRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleModel>>> GetRoles()
        {
            var roles = _context.Roles.Select(r => _mapper.Map<RoleModel>(r)).ToList();
            return roles;
        }

        // GET: api/ApplicationRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleModel>> GetRole(string id)
        {
            var applicationRole = await _context.Roles.FindAsync(id);

            if (applicationRole == null)
            {
                return NotFound();
            }

            return _mapper.Map<RoleModel>(applicationRole);
        }

        // PUT: api/ApplicationRoles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, [Bind("Id,Name,Description")] RoleModel role)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (id == null) { BadRequest(); }

            if (id != role.Id) { return BadRequest(); }

            var applicationRole = await _roleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            applicationRole.Name = role.Name;
            applicationRole.Description = role.Description;

            var response = await _roleManager.UpdateAsync(applicationRole);            
            if (!response.Succeeded) { BadRequest(); }

            return Ok();
        }

        // POST: api/ApplicationRoles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("register")]
        public async Task<ActionResult<RoleModel>> PostRole([Bind("Name,Description")] RoleModel role)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var checkrole = await _roleManager.RoleExistsAsync(role.Name);
            if (checkrole)
            {
                return BadRequest(new { message = $"el rol {role.Name} ya existe" });
            }

            var response = await _roleManager.CreateAsync(new ApplicationRole { Name = role.Name, Description = role.Description });

            if (!response.Succeeded) { BadRequest(); }

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/ApplicationRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var applicationRole = await _context.Roles.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(applicationRole);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Registra un usuario a un rol especificado
        /// </summary>
        /// <param name="roleId">Id del rol</param>
        /// <param name="userId">Id del usuario a registrar</param>        
        /// <returns></returns>     
        [HttpPost("registerUser")]
        public async Task<IActionResult> AddUserToRoleAsync(string roleId, string userId)
        {
            if(roleId == null)
            {
                return BadRequest();
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) { return NotFound(new { message = "No se encontró el rol" }); }

            var user = await _context.Users.FirstAsync(u => u.Id == userId);
            if(user == null) { return NotFound(new { message = "No se encontró el usuario" }); }

            try
            {
                _context.UserRoles.Add(new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id });
                _context.SaveChanges();
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        /// <summary>
        /// Remueve un usuario de un rol especificado
        /// </summary>
        /// <param name="roleId">Id del rol</param>
        /// <param name="userId">Id del usuario a registrar</param>        
        /// <returns></returns>        
        [HttpPost("unregisterUser")]
        public async Task<IActionResult>RemoveUserToRoleAsync(string roleId, string userId )
        {
            if(roleId == null)
            {
                return BadRequest();
            }


            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) { return NotFound(new { message = "No se encontró el rol" }); }

            var userinrol =await _context.UserRoles.FirstAsync(ur => ur.UserId == userId && ur.RoleId == role.Id);
            if(userinrol == null)
            {
                return NotFound(new { message = "no se encontró la información vinculada a los datos parametrizados" });
            }

            try
            {
                _context.UserRoles.Remove(userinrol);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

      
    }
}
