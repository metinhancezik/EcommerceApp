using EntityLayer.Concrete;
using ServiceLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstract
{
    public interface ICartItemsService : IGenericService<CartItems>
    {
        Task<List<CartItems>> GetCartItemsByCartId(long cartId);
        Task<List<CartItems>> GetByCartAndProductIds(long cartId, List<long> productIds);
        Task DeleteCartItems(List<CartItems> cartItems);
        Task<CartItems> GetSingleCartItem(long cartId, long productId);
    }
}
