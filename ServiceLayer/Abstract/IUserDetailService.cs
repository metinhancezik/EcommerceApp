using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstract
{
    public interface IUserDetailService : IGenericService<UserDetails>
    {
        UserDetails GetUserByMail(string email);
        UserDetails GetUserByLongId(long id);
    }
}
