using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using DataAccesLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.EntityFramework
{
    public class EfOrderInformations : GenericRepository<OrderInformations>, IOrderInformations
    {
        public async Task<OrderInformations> GetLastOrderByUserId(long userId)
        {
            using var context = new Context();
            return await context.OrderInformations
         .Include(o => o.Country)
         .Include(o => o.City)
         .Include(o => o.District)
         .Include(o => o.Neighborhood)
         .Where(o => o.UserId == userId)
         .OrderByDescending(o => o.CreatedTime)
         .FirstOrDefaultAsync();
        }
    }
}
