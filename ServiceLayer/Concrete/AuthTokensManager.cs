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
    public class AuthTokensManager : IAuthTokensService
    {
        IAuthTokens _authTokens;

        public AuthTokensManager(IAuthTokens authTokens)
        {
            _authTokens = authTokens;
        }
        public AuthTokens GetById(int id)
        {
            return _authTokens.GetByID(id);
        }

        public Task<long?> GetUserIdFromTokenAsync(string token)
        {
            return _authTokens.GetUserIdFromTokenAsync(token);
        }

        public List<AuthTokens> GetList()
        {
            return _authTokens.GetListAll();
        }

        public void TAdd(AuthTokens t)
        {
            _authTokens.Insert(t);
        }

        public void TDelete(AuthTokens t)
        {
            _authTokens.Delete(t);
        }

        public void TUpdate(AuthTokens t)
        {
            _authTokens.Update(t);
        }
    }
}
