using DataAccesLayer.Abstract;
using EntityLayer.Concrete;
using ServiceLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace ServiceLayer.Concrete
{
    public class RolesManager : IRolesService
    {

        IRolesService _roles;

        public Roles GetById(int id)
        {
            return _roles.GetById(id);
        }

        public List<Roles> GetList()
        {
            return _roles.GetList();
        }

        public void TAdd(Roles t)
        {
            _roles.TAdd(t);
        }

        public void TDelete(Roles t)
        {
            _roles.TDelete(t);
        }

        public void TUpdate(Roles t)
        {
           _roles.TUpdate(t);
        }
    }
}
