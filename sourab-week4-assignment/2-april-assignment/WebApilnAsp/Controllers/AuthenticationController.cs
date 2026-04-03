using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApilnAsp.Models;

namespace WebApilnAsp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
                return BadRequest("User already exists");

            var roles = model.Roles
                .Where(role => !string.IsNullOrWhiteSpace(role))
                .Select(role => role.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (roles.Count == 0)
                return BadRequest("At least one valid role is required");

            IdentityUser user = new()
            {
                Email = model.Email,
                UserName = model.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(500, "User creation failed");

            // Add roles
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRolesAsync(user, roles);

            return Ok("User created successfully");
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username!);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                    authClaims.Add(new Claim(ClaimTypes.Role, role));

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }

        // TOKEN METHOD
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var secret = _configuration["JWT:Secret"];

            if (string.IsNullOrEmpty(secret))
                throw new Exception("JWT Secret missing");

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            return Ok(new Response { Status = "Success", Message = "Logged out successfully" });
        }
    }
}
