using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restoran.DAL;
using Restoran.Models;
using System.Diagnostics.Metrics;

namespace Restoran.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
   
    public class ChefController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ChefController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var chefs = _context.Chefs.ToList();
            return View(chefs);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Chef chef,IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(chef is null)
            {
                ModelState.AddModelError("", "butun fieldler dolu gelmelidi");
            }
            string filename = Guid.NewGuid() + file.FileName;
            var path = Path.Combine(_env.WebRootPath, "Upload", filename);

            using FileStream fileStream = new FileStream(path, FileMode.Create);

            await file.CopyToAsync(fileStream);

            chef.PhotoUrl = "/Upload/" + filename;

            await _context.AddAsync(chef);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var chef=_context.Chefs.FirstOrDefault(x => x.Id == id);
            return View(chef);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Chef chef, IFormFile? file,int id)
        {
            var oldChef = _context.Chefs.FirstOrDefault(x => x.Id == id);
            if(!ModelState.IsValid)
            {
                return View();
            }

            if (oldChef is null)
            {
                return NotFound();
            }
           
            oldChef.FullName = chef.FullName;
            oldChef.Position = chef.Position;
            
            if(file is not null)
            {
               
                string filename = Guid.NewGuid() + file.FileName;
                var path = Path.Combine(_env.WebRootPath, "Upload", filename);

                using FileStream fileStream = new FileStream(path, FileMode.Create);

                await file.CopyToAsync(fileStream);

                chef.PhotoUrl = "/Upload/" + filename;
                oldChef.PhotoUrl = chef.PhotoUrl;

            }

            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var chef = _context.Chefs.FirstOrDefault(x => x.Id == id);
            if (chef is null)
            {
                return NotFound();
            }
            _context.Chefs.Remove(chef);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }   
    }
}
