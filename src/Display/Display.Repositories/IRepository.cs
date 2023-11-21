using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync(string tenantId, string id);
    }
}
