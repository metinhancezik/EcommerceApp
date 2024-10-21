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
    public class OrderStateManager : IOrderStateService
    {
        IOrderState _orderState;


        public OrderStateManager(IOrderState orderState)
        {
            _orderState = orderState;
        }
        public OrderState GetById(int id)
        {
            return _orderState.GetByID(id);
        }

        public List<OrderState> GetList()
        {
            return _orderState.GetListAll();
        }

        public void TAdd(OrderState t)
        {
            _orderState.Insert(t);
        }

        public void TDelete(OrderState t)
        {
            _orderState.Delete(t);
        }

        public void TUpdate(OrderState t)
        {
            _orderState.Update(t);
        }
    }
}
