using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStore.Models
{
    public class Order
    {
        private readonly CapContext _appDbContext;
        private Order(CapContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public int Id { get; set; }
        
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public decimal TotalCost { get; set; }
        public string Status { get; set; }

        public DateTime OrderPlaced { get; set; }

        public List<OrderCap> OrderCaps { get; set; }
        public Order()
        {
            OrderCaps = new List<OrderCap>();
        }

        public List<OrderCap> GetOrderCaps()
        {
            return OrderCaps ??
                   (OrderCaps =
                       _appDbContext.OrderCaps.Where(c => c.OrderId == Id)
                           .Include(s => s.Cap)
                           .ToList());
        }
    }
}
