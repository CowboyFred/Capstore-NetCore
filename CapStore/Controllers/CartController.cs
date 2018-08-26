using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CapStore.Models;
using CapStore.ViewModels;
using CapStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace CapStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICapRepository _capRepository;
        private readonly Cart _cart;

        public CartController(ICapRepository capRepository, Cart cart)
        {
            _capRepository = capRepository;
            _cart = cart;
        }

        [Authorize]
        public ViewResult Index()
        {
            var items = _cart.GetCartCaps();
            _cart.CartCaps = items;

            var cartViewModel = new CartViewModel
            {
                Cart = _cart,
                CartTotal = _cart.GetCartTotal()
            };
            return View(cartViewModel);
        }

        [Authorize]
        public RedirectToActionResult AddToCart(int capId)
        {
            var selectedCap = _capRepository.Caps.FirstOrDefault(p => p.Id == capId);
            if (selectedCap != null)
            {
                _cart.AddToCart(selectedCap, 1);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public RedirectToActionResult RemoveOne(int capId)
        {
            var selectedCap = _capRepository.Caps.FirstOrDefault(p => p.Id == capId);
            if (selectedCap != null)
            {
                _cart.RemoveOne(selectedCap, 1);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public RedirectToActionResult RemoveFromCart(int capId)
        {
            var selectedCap = _capRepository.Caps.FirstOrDefault(p => p.Id == capId);
            if (selectedCap != null)
            {
                _cart.RemoveFromCart(selectedCap);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public RedirectToActionResult ClearCart()
        {
            if (_cart != null)
            {
                _cart.ClearCart();
            }
            return RedirectToAction("Index");
        }

    }
}