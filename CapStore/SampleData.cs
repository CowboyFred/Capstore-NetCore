using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapStore.Models;

namespace CapStore
{
    public class SampleData
    {
        public static void Initialize(CapContext context)
        {
            if (!context.Caps.Any())
            {
                context.Caps.AddRange(
                    new Cap
                    {
                        Name = "Cap2323",
                        SupplierId = 1,
                        Price = 600
                    },
                    new Cap
                    {
                        Name = "Cap2t5",
                        SupplierId = 1,
                        Price = 899
                    },
                    new Cap
                    {
                        Name = "Cap23",
                        SupplierId = 1,
                        Price = 678
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

