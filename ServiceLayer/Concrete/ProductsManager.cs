using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
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
    public class ProductsManager : IProductsService
    {
        IProducts _products;


        public ProductsManager(IProducts products)
        {
            _products = products;
        }

        public Products GetById(int id)
        {
            return _products.GetByID(id);
        }


        public List<Products> GetList()
        {
            return _products.GetListAll();
        }

        public void TAdd(Products t)
        {
            _products.Insert(t);
        }

        public void TDelete(Products t)
        {
            _products.Delete(t);
        }

        public void TUpdate(Products t)
        {
            _products.Update(t);
        }
    }
}
