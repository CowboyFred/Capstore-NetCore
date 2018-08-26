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
    [Authorize(Roles = "admin")]
    public class EditOrdersController : Controller
    {
        private CapContext db;
        IHostingEnvironment _appEnvironment;

        public EditOrdersController(CapContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(string name, int page = 1, SortState sortOrder = SortState.DateDesc)
        {
            IQueryable<Order> orders = db.Orders
                .Include(x => x.Customer)
                .AsNoTracking();

            ViewData["CurrentName"] = name;
            ViewData["CurrentSort"] = sortOrder;

            if (!String.IsNullOrEmpty(name))
            {
                orders = orders.Where(p => p.Customer.UserName.Contains(name));
            }
            
            ViewData["DateSort"] = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            ViewData["UserNameSort"] = sortOrder == SortState.UserNameAsc ? SortState.UserNameDesc : SortState.UserNameAsc;


            switch (sortOrder)
            {
                case SortState.UserNameDesc:
                    orders = orders.OrderByDescending(s => s.Customer.UserName);
                    break;
                case SortState.UserNameAsc:
                    orders = orders.OrderBy(s => s.Customer.UserName);
                    break;
                case SortState.DateDesc:
                    orders = orders.OrderByDescending(s => s.OrderPlaced);
                    break;
                case SortState.DateAsc:
                    orders = orders.OrderBy(s => s.OrderPlaced);
                    break;
            }

            int pageSize = 10;   // elements per page

            var count = await orders.CountAsync();
            var items = await orders.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            OrdersViewModel viewModel = new OrdersViewModel
            {
                Orders = orders.ToList(),
                PageViewModel = pageViewModel
            };

            ViewBag.statusList = viewModel.Statuses;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int? id, string status)
        {
            if (id != null)
            {
                Order order = await db.Orders.FirstOrDefaultAsync(p => p.Id == id);

                order.Status = status;

                db.Orders.Update(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Order order = await db.Orders
                    .Include(c => c.Customer)
                    .Include(c => c.OrderCaps)
                        .ThenInclude(y => y.Cap)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(m => m.Id == id);

                List<Customer> customerList = new List<Customer>();
                customerList = db.Customers.ToList();
                ViewBag.customerList = new SelectList(customerList, "Id", "UserName");

                if (order != null)
                    return View(order);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            Order changeOrder = await db.Orders.FirstOrDefaultAsync(p => p.Id == order.Id);
            changeOrder.CustomerId = order.CustomerId;
            db.Orders.Update(changeOrder);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Order order = await db.Orders
                    .Include(c => c.Customer)
                    .Include(c => c.OrderCaps)
                        .ThenInclude(y => y.Cap)
                    .AsNoTracking() 
                    .SingleOrDefaultAsync(m => m.Id == id);
                if (order != null)
                    return View(order);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Order order = new Order { Id = id.Value };

                IQueryable<OrderCap> orderCaps = db.OrderCaps
                .Where(p => p.OrderId == id)
                .AsNoTracking();

                foreach (OrderCap orderCap in orderCaps)
                {
                    db.OrderCaps.Remove(orderCap);
                }
                db.Orders.Remove(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
