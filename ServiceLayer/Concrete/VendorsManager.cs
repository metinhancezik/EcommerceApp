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
    public class VendorsManager : IVendorsService
    {
        IVendors _vendors;


        public VendorsManager(IVendors vendorDetail)
        {
            _vendors = vendorDetail;
        }

        public Vendors GetById(int id)
        {
            return _vendors.GetByID(id);
        }


        public List<Vendors> GetList()
        {
            return _vendors.GetListAll();
        }

        public void TAdd(Vendors t)
        {
            _vendors.Insert(t);
        }

        public void TDelete(Vendors t)
        {
            _vendors.Delete(t);
        }

        public void TUpdate(Vendors t)
        {
            _vendors.Update(t);
        }

    }
}
