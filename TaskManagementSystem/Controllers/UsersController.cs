using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.MappingExtensionMethods;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAll();
                if (users is null) return NotFound("No User found");
                return Ok(users.Select(d=> d.MapToUserDTO()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a users from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _userService.GetById(userId);
                if (user == null)
                {
                    return NotFound("No User found");
                }
                return Ok(user.MapToUserDTO());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a user with Id:{0} from the database : {1} : {2}", userId, ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpGet("{teamId}/team")]
        [Authorize(Roles = "Admin, Manager, Team Leader")]
        public async Task<IActionResult> GetUserByTeamId(int teamId)
        {
            try
            {
                var users = await _userService.GetUsersByTeam(teamId);
                if (users == null)
                {
                    return NotFound("No User found");
                }
                return Ok(users.Select(d => d.MapToUserDTO()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a user with Id:{0} from the database : {1} : {2}", teamId, ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateUser([FromBody] RequestCreateUserDTO createUser)
        {
            try
            {

                if (createUser is null) return BadRequest();

                var createdUser = await _userService.AddUser(createUser);
                if (createdUser is null) return BadRequest();

                return CreatedAtAction(nameof(CreateUser), new { id = createdUser!.Id }, createdUser.MapToUserDTO());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while creating a user from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] RequestUpdateUserDTO updateUser)
        {
            try
            {
                if (userId != updateUser.Id)
                {
                    return BadRequest();
                }

                var updatedUser = await _userService.UpdateUser(updateUser);
                if (updatedUser is null) return BadRequest();

                return NoContent();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while Updating a user from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = _userService.GetById(id);
            if (existingUser is null) return NotFound();

            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
