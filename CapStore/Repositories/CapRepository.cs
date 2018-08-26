using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapStore.Models;
using CapStore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CapStore.Repositories
{
    public class CapRepository : ICapRepository
    {
        private readonly CapContext _appDbContext;
        public CapRepository(CapContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Cap> Caps => _appDbContext.Caps.Include(c => c.Category);
        public Cap GetCapById(int capId) => _appDbContext.Caps.FirstOrDefault(p => p.Id == capId);
    }
}
