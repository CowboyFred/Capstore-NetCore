using CapStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapStore.Interfaces
{
    public interface ICapRepository
    {
        IEnumerable<Cap> Caps { get; }
        Cap GetCapById(int capId);
    }
}
