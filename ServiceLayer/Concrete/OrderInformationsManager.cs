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
    public class OrderInformationsManager : IOrderInformationsService
    {
        IOrderInformations _orderInformations;

        public OrderInformationsManager(IOrderInformations orderInformations)
        {
            _orderInformations = orderInformations;
        }
        public OrderInformations GetById(int id)
        {
            return _orderInformations.GetByID(id);
        }

        public Task<OrderInformations> GetLastOrderByUserId(long id)
        {
            return _orderInformations.GetLastOrderByUserId(id);
        }

        public List<OrderInformations> GetList()
        {
            return _orderInformations.GetListAll();
        }

        public void TAdd(OrderInformations t)
        {
            _orderInformations.Insert(t);
        }

        public void TDelete(OrderInformations t)
        {
            _orderInformations.Delete(t);
        }

        public void TUpdate(OrderInformations t)
        {
            _orderInformations.Update(t);
        }
    }
}
