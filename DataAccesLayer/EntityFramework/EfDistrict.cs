using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using DataAccesLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.EntityFramework
{
    public class EfDistrict : GenericRepository<District>, IDistrict
    {
        public List<District> GetDistrictsByCityId(int cityId)
        {
            using var context = new Context();
            return context.Set<District>()
                .Where(d => d.CityId == cityId)
                .ToList();
        }

        public async Task<List<District>> GetDistrictsByCityIdAsync(int cityId)
        {
            using var context = new Context();
            return await context.Set<District>()
                .Where(d => d.CityId == cityId)
                .ToListAsync();
        }
    }
}
