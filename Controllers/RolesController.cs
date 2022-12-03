using JwtApplication.Data.models;
using JwtApplication.Repository.Interfaces;
using JwtApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private IUnitOfWork _unitOfWork;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleViewModel>>> GetRoles()
        {
            var all = await _unitOfWork.RoleRepo.FindAll();
            return Ok(all);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _unitOfWork.RoleRepo.FindById(id);

            if (role == null)
            {
                return NotFound("id: " + id + " Not found.");
            }

            return role;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleViewModel roleVm)
        {

            if (!RoleExists(id))
            {
                return NotFound("id: " + id + " Not found.");
            }
            try
            {
                Role role = await _unitOfWork.RoleRepo.FindById(id);
                role.Name = roleVm.Name;
                role.Description = roleVm.Description;

                await _unitOfWork.RoleRepo.Update(role);
                await _unitOfWork.CommitAsync();
                return Ok("Updated");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(RoleViewModel roleVm)
        {
            Role role = new Role()
            {
                Name = roleVm.Name,
                Description = roleVm.Description,

            };
            await _unitOfWork.RoleRepo.Add(role);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _unitOfWork.RoleRepo.FindById(id);
            if (role == null)
            {
                return NotFound("id: " + id + " Not found.");
            }

            await _unitOfWork.RoleRepo.Delete(role);
            await _unitOfWork.CommitAsync();

            return Content("Deleted");
        }

        private bool RoleExists(int id)
        {
            return _unitOfWork.RoleRepo.IsExists(id).Result;
        }



    }
}
