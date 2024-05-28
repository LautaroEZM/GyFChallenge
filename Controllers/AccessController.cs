using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GyFChallenge.Custom;
using GyFChallenge.Models;
using GyFChallenge.Models.DTOs;
using GyFChallenge.Data;
using Microsoft.AspNetCore.Authorization;

namespace GyFChallenge.Controllers // Asegúrate de que este espacio de nombres coincida
{
    [Route("")]
    [AllowAnonymous]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly Utilities _utilities;

        public AccessController(AppDBContext dbContext, Utilities utilities)
        {
            _dbContext = dbContext;
            _utilities = utilities;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDTO element)
        {
            Console.WriteLine("Register user.");
            var userModel = new User
            {
                Username = element.Username,
                Email = element.Email,
                Password = _utilities.EncriptSHA256(element.Password)
            };

            try {
                await _dbContext.Set<User>().AddAsync(userModel);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, new { isSuccess = true });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
                
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            Console.WriteLine("Login user.");
            var user = await _dbContext.Set<User>()
                .Where(u => u.Email == objeto.Email && u.Password == _utilities.EncriptSHA256(objeto.Password))
                .FirstOrDefaultAsync();


            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, new { message = "User not found" });
            else
                return StatusCode(StatusCodes.Status200OK, new { token = _utilities.generateJWT(user) });
        }
    }
}
