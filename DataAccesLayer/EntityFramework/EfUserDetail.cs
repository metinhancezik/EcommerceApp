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
    public class EfUserDetail : GenericRepository<UserDetails>, IUserDetail
    {
        public UserDetails GetUserByLongId(long id)
        {
            using var context=new Context();

            return context.Set<UserDetails>().FirstOrDefault(u => u.Id == id);
        }

        public UserDetails GetUserByMail(string email)
        {
            using var context = new Context();

            
            var userDetails = context.Set<UserDetails>().FirstOrDefault(ud => ud.UserMail == email);

            if (userDetails == null)
            {
                return null; 
            }  
            return userDetails;
        }
    }
}
