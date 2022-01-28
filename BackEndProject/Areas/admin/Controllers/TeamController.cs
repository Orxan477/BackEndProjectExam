using AutoMapper;
using BackEndProject.Utilities;
using BackEndProject.ViewModels.Team;
using Core.Models;
using Data.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProject.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class TeamController : Controller
    {
        private IMapper _mapper;
        private IWebHostEnvironment _env;
        private AppDbContext _context;
        private string _errorMessage;

        public TeamController(AppDbContext context, IWebHostEnvironment env,IMapper mapper)
        {
            _mapper = mapper;
            _env = env;
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Teams.Where(x=>!x.IsDeleted).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamVM createTeam)
        {
            if (!ModelState.IsValid) return View(createTeam);
            bool isExistName = await _context.Teams.AnyAsync(x => x.Name.Trim().ToLower() == createTeam.Name.Trim().ToLower());
            if (isExistName)
            {
                ModelState.AddModelError("Name", "This Name is currently use");
                return View(createTeam);
            }
            if (!CheckImageValid(createTeam.Photo, "image/", 200))
            {
                ModelState.AddModelError("Photo", _errorMessage);
                return View(createTeam);
            }
            string fileName=await Extension.SaveFileAsync(createTeam.Photo, _env.WebRootPath, "assets/img");
            Team team = new Team
            {
                Name = createTeam.Name,
                Position = createTeam.Position,
                Image = fileName
            };
            await _context.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CheckImageValid(IFormFile file,string type,int size)
        {
            if (!Extension.CheckSize(file,size))
            {
                _errorMessage = "The size of this photo is 200";
                return false;
            }
            if (!Extension.CheckType(file,type))
            {
                _errorMessage = "The type is not correct";
                return false;
            }
            return true;
        }
        public IActionResult Update(int id)
        {
            Team dbTeam = _context.Teams.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
            if (dbTeam is null) return NotFound();
            UpdateTeamVM team = _mapper.Map<UpdateTeamVM>(dbTeam);
            return View(team);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,UpdateTeamVM updateTeam)
        {
            if (!ModelState.IsValid) return View(updateTeam);
            Team dbTeam = _context.Teams.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
            bool isCurrentName = dbTeam.Name.Trim().ToLower() == updateTeam.Name.ToLower().Trim();
            if (!isCurrentName)
            {
                dbTeam.Name = updateTeam.Name;
            }
            bool isCurrentPosition = dbTeam.Position.Trim().ToLower() == updateTeam.Position.ToLower().Trim();
            if (!isCurrentPosition)
            {
                dbTeam.Position = updateTeam.Position;
            }
            if (!CheckImageValid(updateTeam.Photo, "image/", 200))
            {
                ModelState.AddModelError("Photo", _errorMessage);
                return View(updateTeam);
            }
            Helper.RemoveFile(_env.WebRootPath, "assets/img", dbTeam.Image);
            string fileName = await Extension.SaveFileAsync(updateTeam.Photo, _env.WebRootPath, "assets/img");
            dbTeam.Image = fileName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Team dbTeam = _context.Teams.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
            if (dbTeam is null) return NotFound();
            //dbTeam.IsDeleted = true;
            Helper.RemoveFile(_env.WebRootPath, "assets/img", dbTeam.Image);
            _context.Teams.Remove(dbTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
