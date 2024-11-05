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
    public class EfCartItems : GenericRepository<CartItems>, ICartItems
    {
        public async Task<List<CartItems>> GetCartItemsByCartId(long cartId)
        {
            using var context = new Context();
            return await context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }
    }
}
