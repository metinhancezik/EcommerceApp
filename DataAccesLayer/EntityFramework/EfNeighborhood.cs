using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using DataAccesLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.EntityFramework
{
    public class EfNeighborhood : GenericRepository<Neighborhood>, INeighborhood
    {
        public List<Neighborhood> GetNeighborhoodsByDistrictId(int districtId)
        {
            using var context = new Context();
            return context.Set<Neighborhood>()
                .Where(c => c.DistrictId == districtId)
                .ToList();
        }
    }
}
