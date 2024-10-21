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
    public class DistrictManager : IDistrictService
    {
        IDistrict _district;

        public DistrictManager(IDistrict district)
        {
            _district = district;
        }
        public District GetById(int id)
        {
            return _district.GetByID(id);
        }

        public List<District> GetList()
        {
            return _district.GetListAll();
        }

        public void TAdd(District t)
        {
            _district.Insert(t);
        }

        public void TDelete(District t)
        {
            _district.Delete(t);
        }

        public void TUpdate(District t)
        {
            _district.Update(t);
        }
    }
}
