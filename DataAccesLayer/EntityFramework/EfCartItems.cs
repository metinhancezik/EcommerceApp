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
        public async Task DeleteCartItems(List<CartItems> cartItems)
        {
            using var context = new Context();
            if (cartItems != null && cartItems.Any())
            {
                context.CartItems.RemoveRange(cartItems);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<CartItems>> GetByCartAndProductIds(long cartId, List<long> productIds)
        {
            using var context = new Context();
            return await context.CartItems
              .Where(ci => ci.CartId == cartId && productIds.Contains(ci.ProductId))
              .ToListAsync();
        }

        public async Task<CartItems> GetSingleCartItem(long cartId, long productId)
        {
            using var context = new Context();
            return await context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
        public async Task<List<CartItems>> GetCartItemsByCartId(long cartId)
        {
            using var context = new Context();
            return await context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }
    }
}
