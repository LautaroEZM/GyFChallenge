using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GyFChallenge.Custom;
using GyFChallenge.Models;
using GyFChallenge.Models.DTOs;
using System.Threading.Tasks;
using GyFChallenge.Data;
using Microsoft.AspNetCore.Authorization;

namespace GyFChallenge.Controllers // Asegúrate de que este espacio de nombres coincida
{
    [Route("api/[controller]")]
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
            var userModel = new User
            {
                Username = element.Username,
                Mail = element.Mail,
                Password = _utilities.EncriptSHA256(element.Password)
            };

            await _dbContext.Set<User>().AddAsync(userModel);
            await _dbContext.SaveChangesAsync();

            if (userModel.Id != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var userFound = await _dbContext.Set<User>()
                .Where(u => u.Mail == objeto.Mail && u.Password == _utilities.EncriptSHA256(objeto.Password))
                .FirstOrDefaultAsync();

            if (userFound == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "User not found" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilities.generateJWT(userFound) });
        }
    }
}
