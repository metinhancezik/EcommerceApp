using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using EntityLayer.Concrete;
using ServiceLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServiceLayer.Concrete
{
 
        public class UserDetailManager : IUserDetailService
        {
        IUserDetail _userDetail;

        public UserDetailManager(IUserDetail userDetail)
        {
            _userDetail = userDetail;
        }

        public UserDetails GetById(int id)
        {
            return _userDetail.GetByID(id);
        }

       
        public List<UserDetails> GetList()
        {
            return _userDetail.GetListAll();
        }

        public void TAdd(UserDetails t)
        {
            _userDetail.Insert(t);
        }

        public void TDelete(UserDetails t)
        {
            _userDetail.Delete(t);
        }

        public void TUpdate(UserDetails t)
        {
            _userDetail.Update(t);
        }
    }
    
}
