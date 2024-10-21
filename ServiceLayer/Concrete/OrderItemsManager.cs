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
    public class OrderItemsManager : IOrderItemsService
    {
        IOrderItems _orderItem;

        public OrderItemsManager(IOrderItems orderItems)
        {
            _orderItem = orderItems;
        }
        public OrderItems GetById(int id)
        {
            return _orderItem.GetByID(id);
        }

        public List<OrderItems> GetList()
        {
            return _orderItem.GetListAll();
        }

        public void TAdd(OrderItems t)
        {
            _orderItem.Insert(t);
        }

        public void TDelete(OrderItems t)
        {
            _orderItem.Delete(t);
        }

        public void TUpdate(OrderItems t)
        {
            _orderItem.Update(t);
        }
    }
}
