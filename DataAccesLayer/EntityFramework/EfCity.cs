using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using DataAccesLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.EntityFramework
{
    public class EfCity : GenericRepository<City>, ICity
    {
        public List<City> GetCitiesByCountryId(int countryId)
        {
            using var context = new Context();
            return context.Set<City>()
                .Where(c => c.CountryId == countryId)
                .ToList();
        }
    }
}
