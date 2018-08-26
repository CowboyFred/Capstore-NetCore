using System;
using System.Collections.Generic;
using System.Linq;
using CapStore.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CapStore.Models
{
    public class OrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }

        public SelectList Customers { get; set; }
        public SelectList Caps { get; set; }

        public PageViewModel PageViewModel { get; set; }
        public string Name { get; set; }

        public List<SelectListItem> Statuses { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Active", Text = "Active" },
            new SelectListItem { Value = "Cancelled", Text = "Cancelled" },
            new SelectListItem { Value = "Delivered", Text = "Delivered"  },
        };
    }
}
