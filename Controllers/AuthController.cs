using APICatalog.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICatalog.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;

            _signInManager = signInManager;

            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]UserInputDTO userInput)
        {
            var user = new IdentityUser
            {
                UserName = userInput.Email,
                Email = userInput.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, userInput.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            await _signInManager.SignInAsync(user, false);

            return Ok(new
            {
                message = "User registered successfully!"
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginInputDTO userInput)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userInput.Email,
                userInput.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials.");

                return BadRequest(ModelState);
            }

            return Ok(GenerateToken(userInput, "User logged in successfully!"));
        }

        private ActionResult<LoginOutputDTO> GenerateToken(LoginInputDTO userInput, string message)
        {
            if (userInput.Email == null)
            {
                return StatusCode(500, "Something went wrong when generating login token.");
            };

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInput.Email),
                new Claim("meuPet", "pipoca"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (_configuration["Jwt:key"] == null)
            {
                return StatusCode(500, "Something went wrong when generating login token.");
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"] ?? "")
            );

            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (_configuration["TokenConfiguration:ExpireHours"] == null)
            {
                return StatusCode(500, "Something went wrong when generating login token.");
            };

            var expiration = DateTime.UtcNow.AddHours(
                double.Parse(_configuration["TokenConfiguration:ExpireHours"] ?? "")
            );

            var token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credenciais
            );

            return new LoginOutputDTO()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = message
            };
        }
    }
}
