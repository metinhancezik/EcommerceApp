﻿using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.Abstract
{
    public interface IAuthTokens : IGenericDal<AuthTokens>
    {
        Task<long?> GetUserIdFromTokenAsync(string token); 
    }
}
