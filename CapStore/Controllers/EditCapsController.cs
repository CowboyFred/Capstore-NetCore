using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CapStore.Models;
using CapStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;

namespace CapStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class EditCapsController : Controller
    {
        private CapContext db;
        IHostingEnvironment _appEnvironment;

        public EditCapsController(CapContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int? category, int? supplier,  string name, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Cap> caps = db.Caps
                .Include(x => x.Category)
                .Include(x => x.Supplier)
                .AsNoTracking();

            ViewData["CurrentName"] = name;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentSupplier"] = supplier;
            ViewData["CurrentSort"] = sortOrder;

            if (category != null && category != 0)
            {
                caps = caps.Where(p => p.CategoryId == category);
            }

            if (supplier != null && supplier != 0)
            {
                caps = caps.Where(p => p.SupplierId == supplier);
            }

            if (!String.IsNullOrEmpty(name))
            {
                caps = caps.Where(p => p.Name.Contains(name));
            }


            List<Category> categories = db.Categories.ToList();
            List<Supplier> suppliers = db.Suppliers.ToList();
            // set the first element which allow to select all
            categories.Insert(0, new Category { Name = "All", Id = 0 });
            suppliers.Insert(0, new Supplier { Name = "All", Id = 0 });

            
            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["PriceSort"] = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            ViewData["CatSort"] = sortOrder == SortState.CategoryAsc ? SortState.CategoryDesc : SortState.CategoryAsc;
            ViewData["SupSort"] = sortOrder == SortState.SupplierAsc ? SortState.SupplierDesc : SortState.SupplierAsc;

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    caps = caps.OrderByDescending(s => s.Name);
                    break;
                case SortState.PriceAsc:
                    caps = caps.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    caps = caps.OrderByDescending(s => s.Price);
                    break;
                case SortState.SupplierAsc:
                    caps = caps.OrderBy(s => s.Supplier.Name);
                    break;
                case SortState.SupplierDesc:
                    caps = caps.OrderByDescending(s => s.Supplier.Name);
                    break;
                case SortState.CategoryAsc:
                    caps = caps.OrderBy(s => s.Category.Name);
                    break;
                case SortState.CategoryDesc:
                    caps = caps.OrderByDescending(s => s.Category.Name);
                    break;
                default:
                    caps = caps.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;   // elements per page

            var count = await caps.CountAsync();
            var items = await caps.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            CapsListViewModel viewModel = new CapsListViewModel
            {
                Caps = items.ToList(),
                Categories = new SelectList(categories, "Id", "Name"),
                Suppliers = new SelectList(suppliers, "Id", "Name"),
                PageViewModel = pageViewModel,
                Name = name
            };

            return View(viewModel);
        }


        public IActionResult Create()
        {
            List<Category> categoryList = new List<Category>();
            categoryList = db.Categories.ToList();
            ViewBag.categoryList = new SelectList(categoryList, "Id", "Name");

            List<Supplier> supplierList = new List<Supplier>();
            supplierList = db.Suppliers.ToList();
            ViewBag.supplierList = new SelectList(supplierList, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cap cap, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    var relativeName = "";
                    var fileName = "";

                    fileName = ContentDispositionHeaderValue
                                                          .Parse(uploadedFile.ContentDisposition)
                                                          .FileName
                                                          .Trim('"');

                    relativeName = "/images/CapImages/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;

                    using (FileStream fs = System.IO.File.Create(_appEnvironment.WebRootPath + relativeName))
                    {
                        await uploadedFile.CopyToAsync(fs);
                        fs.Flush();
                    }
                    cap.Image = "/eltcod01/asp_application1/" + relativeName;
                }

                db.Caps.Add(cap);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                List<Category> categoryList = new List<Category>();
                categoryList = db.Categories.ToList();
                ViewBag.categoryList = new SelectList(categoryList, "Id", "Name");

                List<Supplier> supplierList = new List<Supplier>();
                supplierList = db.Suppliers.ToList();
                ViewBag.supplierList = new SelectList(supplierList, "Id", "Name");

                return View(cap);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Cap cap = await db.Caps.AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == id);

                List<Category> categoryList = new List<Category>();
                categoryList = db.Categories.ToList();
                ViewBag.categoryList = new SelectList(categoryList, "Id", "Name");

                List<Supplier> supplierList = new List<Supplier>();
                supplierList = db.Suppliers.ToList();
                ViewBag.supplierList = new SelectList(supplierList, "Id", "Name");

                if (cap != null)
                    return View(cap);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cap cap, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                Cap changeCap = await db.Caps.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == cap.Id);
                if (uploadedFile != null)
                {
                    if (System.IO.File.Exists(_appEnvironment.WebRootPath + changeCap.Image))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + changeCap.Image);
                    }
                    var relativeName = "";
                    var fileName = "";

                    fileName = ContentDispositionHeaderValue
                                                          .Parse(uploadedFile.ContentDisposition)
                                                          .FileName
                                                          .Trim('"');

                    relativeName = "/images/CapImages/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;

                    using (FileStream fs = System.IO.File.Create(_appEnvironment.WebRootPath + relativeName))
                    {
                        await uploadedFile.CopyToAsync(fs);
                        fs.Flush();
                    }
                    cap.Image = "/eltcod01/asp_application1/" + relativeName;
                }

                else
                {
                    cap.Image = changeCap.Image;
                }

                changeCap = cap;
                db.Caps.Update(changeCap);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            else
            {
                List<Category> categoryList = new List<Category>();
                categoryList = db.Categories.ToList();
                ViewBag.categoryList = new SelectList(categoryList, "Id", "Name");

                List<Supplier> supplierList = new List<Supplier>();
                supplierList = db.Suppliers.ToList();
                ViewBag.supplierList = new SelectList(supplierList, "Id", "Name");

                return View(cap);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Cap cap = await db.Caps
                    .Include(c => c.Supplier)
                    .Include(c => c.Category)
                    .AsNoTracking() 
                    .SingleOrDefaultAsync(m => m.Id == id);
                if (cap != null)
                    return View(cap);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Cap cap = new Cap { Id = id.Value };
                if (System.IO.File.Exists(_appEnvironment.WebRootPath + cap.Image))
                {
                    System.IO.File.Delete(_appEnvironment.WebRootPath + cap.Image);
                }
                db.Caps.Remove(cap);
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
