using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CapStore.Models
{
    public class CartCap
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public Cap Cap { get; set; }
        public int Amount { get; set; }
    }
}
