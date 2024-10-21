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
        public CartItems GetById(int id)
        {
            return _cartItems.GetByID(id);
        }

        public List<CartItems> GetList()
        {
            return _cartItems.GetListAll();
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
