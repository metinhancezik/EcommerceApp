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
    public class CountryManager : ICountryService
    {
        ICountry _country;

        public CountryManager(ICountry country)
        {
            _country = country;
        }
        public Country GetById(int id)
        {
            return _country.GetByID(id);
        }

        public List<Country> GetList()
        {
            return _country.GetListAll();
        }

        public void TAdd(Country t)
        {
            _country.Insert(t);
        }

        public void TDelete(Country t)
        {
            _country.Delete(t);
        }

        public void TUpdate(Country t)
        {
            _country.Update(t);
        }
    }
}
