using DataAccesLayer.Abstract;
using DataAccesLayer.Concrete;
using DataAccesLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.EntityFramework
{
    public class EfAuthTokens : GenericRepository<AuthTokens>, IAuthTokens
    {
        public async Task<long?> GetUserIdFromTokenAsync(string token)
        {
            using var context = new Context();
            var authToken = await context.Set<AuthTokens>()
                .FirstOrDefaultAsync(u => u.AccessToken == token);

            return authToken?.UserId;
        }
    }
}
