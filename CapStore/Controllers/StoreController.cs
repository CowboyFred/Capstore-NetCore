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

namespace CapStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly CapContext db;
        IHostingEnvironment _appEnvironment;

        public StoreController(CapContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int? category, string name, string priceRange, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Cap> caps = db.Caps
                .Include(x => x.Category)
                .Include(x => x.Supplier)
                .AsNoTracking();
            decimal maxPrice = caps.Max(p => p.Price);
            decimal minPrice = caps.Min(p => p.Price);
            caps = caps.Where(p => (p.Visible == true));

            ViewData["CurrentRange"] = priceRange;
            ViewData["CurrentName"] = name;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentSort"] = sortOrder;

            if (!String.IsNullOrEmpty(priceRange))
            {
                //Get ints from string priceRange
                string[] s = priceRange.Split(',');
                int minRange = Convert.ToInt32(s[0]);
                int maxRange = Convert.ToInt32(s[1]);

                caps = caps.Where(p => (p.Price >= minRange && p.Price <= maxRange)); 
            }

            if (category != null && category != 0)
            {
                caps = caps.Where(p => p.CategoryId == category);
            }

            if (!String.IsNullOrEmpty(name))
            {
                caps = caps.Where(p => p.Name.Contains(name));
            }

            List<Category> categories = db.Categories.ToList();
            // set the first element which allow to select all
            categories.Insert(0, new Category { Name = "All", Id = 0 });

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["PriceSort"] = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            ViewData["CatSort"] = sortOrder == SortState.CategoryAsc ? SortState.CategoryDesc : SortState.CategoryAsc;

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

            int pageSize = 9;   // elements per page

            var count = await caps.CountAsync();
            var items = await caps.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            CapsListViewModel viewModel = new CapsListViewModel
            {
                Caps = items.ToList(),
                Categories = new SelectList(categories, "Id", "Name"),
                PageViewModel = pageViewModel,
                Name = name,
                MaxPrice = maxPrice,
                MinPrice = minPrice
            };

            return View(viewModel);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Cap cap = await db.Caps.FirstOrDefaultAsync(p => p.Id == id);

                List<Category> categories = db.Categories.ToList();
                List<Supplier> suppliers = db.Suppliers.ToList();
                // set the first element which allow to select all
                categories.Insert(0, new Category { Name = "All", Id = 0 });
                suppliers.Insert(0, new Supplier { Name = "All", Id = 0 });

                if (cap != null)
                    return View(cap);
            }
            return NotFound();
        }
    }
}
