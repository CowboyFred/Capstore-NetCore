using CapStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CapStore.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace DrinkAndGo.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly CapContext _context;
        private readonly Cart _cart;
        private readonly UserManager<Customer> _userManager;


        public OrderController(CapContext context, Cart cart, UserManager<Customer> userManager)
        {
            _context = context;
            _cart = cart;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var customerId = _userManager.GetUserId(User);

            IQueryable<Order> orders = _context.Orders
                .Where(p => p.CustomerId == customerId)
                .Include(x => x.OrderCaps)
                    .ThenInclude(y => y.Cap)
                .OrderByDescending(x => x.OrderPlaced)
                .AsNoTracking();


            int pageSize = 3;   // elements per page

            var count = await orders.CountAsync();
            var items = await orders.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            OrdersViewModel viewModel = new OrdersViewModel
            {
                Orders = items.ToList(),
                PageViewModel = pageViewModel
            };

            return View(viewModel);
        }

        public IActionResult Checkout()
        {
            var items = _cart.GetCartCaps();
            _cart.CartCaps = items;
            if (_cart.CartCaps.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some caps first");
                ViewBag.CheckoutMessage = "Your cart is empty, add some caps first";
            }

            if (ModelState.IsValid)
            {
                var order = new Order();
                var customerId = _userManager.GetUserId(User);
                decimal totalCost = 0;

                order.OrderPlaced = DateTime.Now;
                order.CustomerId = customerId;
                order.Status = "Active";
                order.TotalCost = _cart.GetCartTotal();

                _context.Orders.Add(order);

                var cartCaps = _cart.CartCaps;

                foreach (var cartCap in cartCaps)
                {
                    var orderCap = new OrderCap()
                    {
                        Quantity = cartCap.Amount,
                        Cap = cartCap.Cap,
                        OrderId = order.Id,
                        Price = cartCap.Cap.Price
                    };

                    totalCost += orderCap.Price * orderCap.Quantity;
                    _context.OrderCaps.Add(orderCap);
                }

                _context.SaveChanges();
                _cart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View();
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thank you for your order! Our managers will contact you soon";
            return View();
        }
    }
}
