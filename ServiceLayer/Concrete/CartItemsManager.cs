using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using DataAccesLayer.EntityFramework;
using DataAccesLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Concrete
{
    public class CartItemsManager : ICartItemsService
    {
        ICartItems _cartItems;

        public CartItemsManager(ICartItems cartItems)
        {
            _cartItems = cartItems;
        }

        public Task DeleteCartItems(List<CartItems> cartItems)
        {
            return _cartItems.DeleteCartItems(cartItems);
        }

        public Task<List<CartItems>> GetByCartAndProductIds(long cartId, List<long> productIds)
        {
            return _cartItems.GetByCartAndProductIds(cartId, productIds);
        }

        public CartItems GetById(int id)
        {
            return _cartItems.GetByID(id);
        }

        public Task<List<CartItems>> GetCartItemsByCartId(long cartId)
        {
            return _cartItems.GetCartItemsByCartId(cartId);
        }

        public List<CartItems> GetList()
        {
            return _cartItems.GetListAll();
        }

        public Task<CartItems> GetSingleCartItem(long cartId, long productId)
        {
           return _cartItems.GetSingleCartItem(cartId, productId);
        }

        public void TAdd(CartItems t)
        {
            _cartItems.Insert(t);
        }

        public void TDelete(CartItems t)
        {
            _cartItems.Delete(t);
        }

        public void TUpdate(CartItems t)
        {
            _cartItems.Update(t);
        }
    }
}
