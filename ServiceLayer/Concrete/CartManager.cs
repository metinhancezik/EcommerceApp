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
    public class CartManager : ICartService
    {
        ICart _cart;

        public CartManager(ICart cart)
        {
            _cart = cart;
        }

        public Task<Cart> GetByUserId(long userId)
        {
            return _cart.GetByUserId(userId);
        }

        public Cart GetById(int id)
        {
            return _cart.GetByID(id);
        }

        public List<Cart> GetList()
        {
            return _cart.GetListAll();
        }

        public void TAdd(Cart t)
        {
            _cart.Insert(t);
        }

        public void TDelete(Cart t)
        {
            _cart.Delete(t);
        }

        public void TUpdate(Cart t)
        {
            _cart.Update(t);
        }
    }
}
