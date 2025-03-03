﻿using EntityLayer.Concrete;
using ServiceLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Abstract
{
    public interface IProductsService : IGenericService<Products>
    {
        Products GetProductByLongId(long id);
    }
}
