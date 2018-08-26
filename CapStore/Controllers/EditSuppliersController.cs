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
    public class EditSuppliersController : Controller
    {
        private CapContext db;
        public EditSuppliersController(CapContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Suppliers.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            else
            {
                return View(supplier);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Supplier supplier = await db.Suppliers.FirstOrDefaultAsync(p => p.Id == id);
                if (supplier != null)
                    return View(supplier);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Update(supplier);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            else
            {
                return View(supplier);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Supplier supplier = await db.Suppliers.FirstOrDefaultAsync(p => p.Id == id);
                if (supplier != null)
                    return View(supplier);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Supplier supplier = new Supplier { Id = id.Value };
                db.Suppliers.Remove(supplier);
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
