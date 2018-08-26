using System;
using System.Collections.Generic;
using System.Linq;
using CapStore.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CapStore.Models
{
    public class CapsListViewModel
    {
        public IEnumerable<Cap> Caps { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Suppliers { get; set; }
        public string Name { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
