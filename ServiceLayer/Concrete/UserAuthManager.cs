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
    public class UserAuthManager : IUserAuthService
    {
        IUserAuth _userAuth;


        public UserAuthManager(IUserAuth userAuth)
        {
            _userAuth = userAuth;
        }

        public UserAuth GetById(int id)
        {
            return _userAuth.GetByID(id);
        }


        public List<UserAuth> GetList()
        {
            return _userAuth.GetListAll();
        }

        public void TAdd(UserAuth t)
        {
            _userAuth.Insert(t);
        }

        public void TDelete(UserAuth t)
        {
            _userAuth.Delete(t);
        }

        public void TUpdate(UserAuth t)
        {
            _userAuth.Update(t);
        }
    }
}
