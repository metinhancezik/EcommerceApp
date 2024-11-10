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
    public class EfUserAuth : GenericRepository<UserAuth>, IUserAuth
    {
    
        public UserAuth GetByUserID(long id)
        {    
            using var context = new Context();

            return context.Set<UserAuth>().FirstOrDefault(u => u.UserId == id);
        }
    }
}
