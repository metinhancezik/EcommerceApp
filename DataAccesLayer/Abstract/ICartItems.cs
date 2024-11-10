using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.Abstract
{
    public interface ICartItems : IGenericDal<CartItems>
    {
        Task<List<CartItems>> GetCartItemsByCartId(long cartId);
        Task<List<CartItems>> GetByCartAndProductIds(long cartId, List<long> productIds);
        Task DeleteCartItems(List<CartItems> cartItems);
        Task<CartItems> GetSingleCartItem(long cartId, long productId);
    }
}
