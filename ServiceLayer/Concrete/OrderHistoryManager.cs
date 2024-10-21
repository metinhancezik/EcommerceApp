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
    public class OrderHistoryManager : IOrderHistoryService
    {
        IOrderHistory _orderHistory;

        public OrderHistoryManager(IOrderHistory orderHistory)
        {
            _orderHistory = orderHistory;
        }
        public OrderHistory GetById(int id)
        {
            return _orderHistory.GetByID(id);

        }

        public List<OrderHistory> GetList()
        {
            return _orderHistory.GetListAll();
        }

        public void TAdd(OrderHistory t)
        {
            _orderHistory.Insert(t);
        }

        public void TDelete(OrderHistory t)
        {
            _orderHistory.Delete(t);
        }

        public void TUpdate(OrderHistory t)
        {
            _orderHistory.Update(t);
        }
    }
}
