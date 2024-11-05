using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using DataAccesLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.EntityFramework
{
    public class EfProducts : GenericRepository<Products>, IProducts
    {
        public Products GetProductByLongId(long id)
        {
            using var context = new Context();

            return context.Set<Products>().FirstOrDefault(u => u.Id == id);
        }
    }
}
