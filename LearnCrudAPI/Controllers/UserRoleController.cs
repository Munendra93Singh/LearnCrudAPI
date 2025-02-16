using LearnCrudAPI.Modal;
using LearnCrudAPI.Repos.Models;
using LearnCrudAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService userRole;

        public UserRoleController(IUserRoleService roleService)
        {
            this.userRole = roleService;
        }
        [HttpPost("assignrolepermission")]
        public async Task<IActionResult> UserRegisteration(List<TblRolepermission>rolepermissions)
        {
            var data = await this.userRole.AssignRolePermission(rolepermissions);
            return Ok(data);
        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult>getAllRoles()
        {
            var data = await this.userRole.GetAllRoles();
            if(data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }
        [HttpGet("GetAllMenus")]
        public async Task<IActionResult> GetAllMenus()
        {
          var data = await this.userRole.GetTblMenus();
            if( data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }

        [HttpGet("GetAllMenusbyrole")]
        public async Task<IActionResult> GetAllMenusbyrole(string userrole)
        {
            var data = await this.userRole.GetAllMenubyrole(userrole);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [HttpGet("GetMenupermissionbyrole")]
        public async Task<IActionResult> GetMenupermissionbyrole(string userrole, string menucode)
        {
            var data = await this.userRole.GetMenupermissionbyrole(userrole, menucode);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

    }
}