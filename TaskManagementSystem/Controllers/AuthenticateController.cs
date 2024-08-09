using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskManagementSystem.AuthenticationHelper;
using TaskManagementSystem.DTOs;
using TaskManagementSystem.DTOs.ResonseDTO;
using TaskManagementSystem.Service.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly JWTSettings _jWTSettings;
        public AuthenticateController(IRefreshTokenService refreshTokenService,
                                      IUserService userService,
                                      ILoginService loginService,
                                      IOptions<JWTSettings> options)
        {
            _refreshTokenService = refreshTokenService;
            _userService = userService;
            _jWTSettings = options.Value;
            _loginService = loginService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] RequestLoginDTO user)
        {
            try
            {
                var loggedInUser = await _loginService.Login(user);
                if (loggedInUser is null) return NotFound("User not found, wrong username or password");

                string tokenString = PasswordManager.GenerateJwtToken(loggedInUser, _jWTSettings);
                string newRefreshToken = PasswordManager.GenerateRefreshToken();

                await _refreshTokenService.InvalidateRefreshTokensForUser(loggedInUser);
                await _refreshTokenService.AddRefreshTokenForUser(loggedInUser, newRefreshToken);

                var tokenResponse = new ResponseTokenDTO
                {
                    JwtToken = tokenString,
                    RefreshToken = newRefreshToken
                };

                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a login from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }
        }

        [Route("refreshtoken")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RequestRefreshTokenDTO refreshTokenDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(refreshTokenDTO.RefreshToken))
                {

                    var refreshToken = await _refreshTokenService.RefreshToken(refreshTokenDTO.RefreshToken);

                    if (refreshToken is null) { return BadRequest("Invalid refresh token"); }

                    var user = await _userService.GetById(refreshToken.UserId);

                    if (user is null) { return BadRequest("Invalid refresh token"); }


                    await _refreshTokenService.InvalidateRefreshTokensForUser(user);

                    await _refreshTokenService.AddRefreshTokenForUser(user, refreshToken.RefreshToken);

                    string tokenString = PasswordManager.GenerateJwtToken(user, _jWTSettings);

                    var tokenResponse = new ResponseTokenDTO
                    {
                        JwtToken = tokenString,
                        RefreshToken = refreshToken.RefreshToken
                    };

                    return Ok(tokenResponse);
                }

                return BadRequest("Invalid refresh token");
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while getting a refresh token from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }

        }

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] RequestRefreshTokenDTO refreshTokenDTO)
        {
            try
            {
                var success = await _loginService.Logout(refreshTokenDTO);
                if (success)
                {
                    return Ok("Successuly logged out");
                }

                return BadRequest("Invalid refresh token");
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("An error occured while doing a logout from the database : {0} : {1}", ex.Message, ex.StackTrace);
                return Problem("An error occured while processing the request.", statusCode: 500);
            }

        }


    }
}
