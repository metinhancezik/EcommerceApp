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
    public class OrderStatusManager : IOrderStatusService
    {
        IOrderStatus _orderStatus;
        public OrderStatusManager(IOrderStatus orderStatus)
        {
            _orderStatus = orderStatus;
        }
        public OrderStatus GetById(int id)
        {
            return _orderStatus.GetByID(id);
        }

        public List<OrderStatus> GetList()
        {
            return _orderStatus.GetListAll();
        }

        public void TAdd(OrderStatus t)
        {
            _orderStatus.Insert(t);
        }

        public void TDelete(OrderStatus t)
        {
            _orderStatus.Delete(t);
        }

        public void TUpdate(OrderStatus t)
        {
            _orderStatus.Update(t);
        }
    }
}
