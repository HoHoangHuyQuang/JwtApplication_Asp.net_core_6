using JwtApplication.Data.models;
using JwtApplication.Repository.Interfaces;
using JwtApplication.Security.payload;
using JwtApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtApplication.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public UserInfoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/UserInfo
        [HttpGet, Route("[controller]")]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfos()
        {
            var all = await _unitOfWork.UserInfoRepo.FindAll();
            return Ok(all);
        }

        // GET: api/UserInfo/5
        [HttpGet("[controller]/{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(int id)
        {
            var userInfo = await _unitOfWork.UserInfoRepo.FindById(id);

            if (userInfo == null)
            {
                return NotFound("id: " + id + " Not found.");
            }

            return userInfo;
        }
        // GET: api/UserInfo/5/roles
        [HttpGet("[controller]/{id}/roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetUserRoles(int id)
        {
            var userInfo = await _unitOfWork.UserInfoRepo.FindById(id);

            if (userInfo == null)
            {
                return NotFound("id: " + id + " Not found.");
            }
            List<Role> result = new List<Role>();
            foreach (UserRole ur in userInfo.UserRoles)
            {
                result.Add(ur.Role);
            }
            return Ok(result);
        }


        // PUT: api/UserInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("[controller]/{id}")]
        public async Task<IActionResult> PutUserInfo(int id, UserInfoViewModel userVm)
        {
            if (!UserInfoExists(id))
            {
                return NotFound("id: " + id + " Not found.");
            }
            if (!userVm.Password.Equals(userVm.ConfirmPassword))
            {
                return BadRequest("Password confirm not equal");
            }

            try
            {
                UserInfo userInfo = await _unitOfWork.UserInfoRepo.FindById(id);
                userInfo.DisplayName = userVm.DisplayName;
                userInfo.Email = userVm.Email;
                userInfo.UserName = userVm.UserName;
                userInfo.Password = userVm.Password;
                userInfo.CreatedDate = userVm.CreatedDate;


                await _unitOfWork.UserInfoRepo.Update(userInfo);
                await _unitOfWork.CommitAsync();
                return Ok("Updated");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/UserInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Route("[controller]")]
        public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfoViewModel userVm)
        {
            if (!userVm.Password.Equals(userVm.ConfirmPassword))
            {
                return BadRequest("Password confirm not equal");
            }
            if (_unitOfWork.UserInfoRepo.IsExistByUsername(userVm.UserName))
            {
                return Content("username is in use!");
            }

            UserInfo userInfo = new UserInfo()
            {
                DisplayName = userVm.DisplayName,
                Email = userVm.Email,
                UserName = userVm.UserName,
                Password = userVm.Password,
                CreatedDate = userVm.CreatedDate,
                UserRoles = new List<UserRole>()
            };
            await _unitOfWork.UserInfoRepo.Add(userInfo);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction("GetUserInfo", new
            {
                id = userInfo.UserId
            }, userInfo);
        }
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet, Route("[controller]/{id}/roles/{roleId}")]
        public async Task<ActionResult<UserInfo>> PostUserRole(int id, int roleId)
        {
            var userToUpdate = await _unitOfWork.UserInfoRepo.FindById(id);
            if (userToUpdate == null)
            {
                return NotFound("User not found!");
            }
            var role = await _unitOfWork.RoleRepo.FindById(roleId);
            if (role == null)
            {
                return NotFound("Role not found!");
            }
            //foreach (UserRole uRole in userToUpdate.UserRoles)
            //{
            //    if (uRole.RoleId == roleId)
            //    {
            //        return Content("Role has already added");
            //    }
            //}
            role.UserRoles = new List<UserRole>();
            role.UserRoles.Add(new UserRole() { Role = role, RoleId = role.Id, UserId = userToUpdate.UserId, UserInfo = userToUpdate });
            await _unitOfWork.RoleRepo.Update(role);
            await _unitOfWork.CommitAsync();
            return Ok("Role added");
        }
        // DELETE: api/UserInfo/5
        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteUserInfo(int id)
        {
            var userInfo = await _unitOfWork.UserInfoRepo.FindById(id);
            if (userInfo == null)
            {
                return NotFound("id: " + id + " Not found.");
            }

            await _unitOfWork.UserInfoRepo.Delete(userInfo);
            await _unitOfWork.CommitAsync();

            return Content("Deleted");
        }

        private bool UserInfoExists(int id)
        {
            return _unitOfWork.UserInfoRepo.IsExists(id).Result;
        }



        [AllowAnonymous]
        [HttpPost, Route("login")]
        public ActionResult<AuthResponse> LoginAuthenticate([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request! Username and password not null!");
            }

            var user = _unitOfWork.UserInfoRepo.GetLoginUser(request);
            if (user == null)
            {
                return Content("incorrect username or password");
            }
            string accessToken = _unitOfWork.TokenRepo.GenerateAccessToken(user);
            string refreshToken = _unitOfWork.TokenRepo.GenerateRefreshToken();

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

    }
}
