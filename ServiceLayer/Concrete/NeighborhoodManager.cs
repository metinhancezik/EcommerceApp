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
    public class NeighborhoodManager : INeighborhoodService
    {
        INeighborhood _neighborhood;

        public NeighborhoodManager(INeighborhood neighborhood)
        {
            _neighborhood = neighborhood;
        }
        public Neighborhood GetById(int id)
        {
            return _neighborhood.GetByID(id);
        }

        public List<Neighborhood> GetList()
        {
            return _neighborhood.GetListAll();
        }

        public void TAdd(Neighborhood t)
        {
            _neighborhood.Insert(t);
        }

        public void TDelete(Neighborhood t)
        {
            _neighborhood.Delete(t);
        }

        public void TUpdate(Neighborhood t)
        {
            _neighborhood.Update(t);
        }
    }
}
