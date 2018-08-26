using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CapStore.Models
{
    public class Cart
    {
        private readonly CapContext _appDbContext;
        private Cart(CapContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string CartId { get; set; }

        public List<CartCap> CartCaps { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<CapContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);
            Cart cart = new Cart(context) { CartId = cartId };

            return cart;
        }

        public void AddToCart(Cap cap, int amount)
        {
            var cartCap =
                    _appDbContext.CartCaps.SingleOrDefault(
                        s => s.Cap.Id == cap.Id && s.CartId == CartId);

            if (cartCap == null)
            {
                cartCap = new CartCap
                {
                    CartId = CartId,
                    Cap = cap,
                    Amount = 1
                };

                _appDbContext.CartCaps.Add(cartCap);
            }
            else
            {
                cartCap.Amount++;
            }
            _appDbContext.SaveChanges();
        }

        public int RemoveOne(Cap cap, int amount)
        {
            var cartCap =
                    _appDbContext.CartCaps.SingleOrDefault(
                        s => s.Cap.Id == cap.Id && s.CartId == CartId);

            var localAmount = 0;

            if (cartCap != null)
            {
                if (cartCap.Amount > 1)
                {
                    cartCap.Amount--;
                    localAmount = cartCap.Amount;
                }
                else
                {
                    _appDbContext.CartCaps.Remove(cartCap);
                }
            }

            _appDbContext.SaveChanges();

            return localAmount;
        }

        public void RemoveFromCart(Cap cap)
        {
            var cartCap =
                    _appDbContext.CartCaps.SingleOrDefault(
                        s => s.Cap.Id == cap.Id && s.CartId == CartId);
            _appDbContext.CartCaps.Remove(cartCap);
            _appDbContext.SaveChanges();

        }

        public List<CartCap> GetCartCaps()
        {
            return CartCaps ??
                   (CartCaps =
                       _appDbContext.CartCaps.Where(c => c.CartId == CartId)
                           .Include(s => s.Cap)
                           .ToList());
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext
                .CartCaps
                .Where(cart => cart.CartId == CartId);

            _appDbContext.CartCaps.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetCartTotal()
        {
            var total = _appDbContext.CartCaps.Where(c => c.CartId == CartId)
                .Select(c => c.Cap.Price * c.Amount).Sum();
            return total;
        }
    }
}
