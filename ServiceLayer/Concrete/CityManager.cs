using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using DataAccesLayer.EntityFramework;
using DataAccesLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Concrete
{
    public class CityManager : ICityService
    {
        ICity _city;

        public CityManager(ICity city)
        {
            _city = city;
        }
        public City GetById(int id)
        {
            return _city.GetByID(id);
        }

        public List<City> GetList()
        {
            return _city.GetListAll();
        }

        public void TAdd(City t)
        {
            _city.Insert(t);
        }

        public void TDelete(City t)
        {
            _city.Delete(t);
        }

        public void TUpdate(City t)
        {
            _city.Update(t);
        }
    }
}
