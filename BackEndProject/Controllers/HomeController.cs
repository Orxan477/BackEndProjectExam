using Data.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BackEndProject.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Teams.Where(x=>!x.IsDeleted).ToList());
        }
    }
}
