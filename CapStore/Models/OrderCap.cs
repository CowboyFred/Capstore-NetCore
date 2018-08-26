using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapStore.Models
{
    public class OrderCap
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Cap Cap { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
