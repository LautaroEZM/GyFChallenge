using Microsoft.AspNetCore.Mvc;

using GyFChallenge.Data;
using GyFChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace GyFChallenge.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _appDbContext;

        public ProductController(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
