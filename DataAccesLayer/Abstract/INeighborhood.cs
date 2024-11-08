using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.Abstract
{
    public interface INeighborhood : IGenericDal<Neighborhood>
    {
        List<Neighborhood> GetNeighborhoodsByDistrictId(int id);
    }
}
