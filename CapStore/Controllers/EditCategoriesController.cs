using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CapStore.Models;
using CapStore.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace CapStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class EditCategoriesController : Controller
    {
        private CapContext db;
        public EditCategoriesController(CapContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            else
            {
                return View(category);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Category category = await db.Categories.FirstOrDefaultAsync(p => p.Id == id);
                if (category != null)
                    return View(category);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Update(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            else
            {
                return View(category);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Category category = await db.Categories.FirstOrDefaultAsync(p => p.Id == id);
                if (category != null)
                    return View(category);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Category category = new Category { Id = id.Value };
                db.Categories.Remove(category);
                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)                  
                    return RedirectToAction("DelError");
                }
            }
            return NotFound();
        }

        public IActionResult DelError()
        {
            return View();
        }
    }
}
