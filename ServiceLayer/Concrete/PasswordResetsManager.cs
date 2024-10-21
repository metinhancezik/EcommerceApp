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
    public class PasswordResetsManager : IPasswordResetsService
    {
        IPasswordResets _passwordResets;

        public PasswordResetsManager(IPasswordResets passwordResets)
        {
            _passwordResets = passwordResets;
        }

        public PasswordResets GetById(int id)
        {
            return _passwordResets.GetByID(id);
        }


        public List<PasswordResets> GetList()
        {
            return _passwordResets.GetListAll();
        }

        public void TAdd(PasswordResets t)
        {
            _passwordResets.Insert(t);
        }

        public void TDelete(PasswordResets t)
        {
            _passwordResets.Delete(t);
        }

        public void TUpdate(PasswordResets t)
        {
            _passwordResets.Update(t);
        }

    }
}
