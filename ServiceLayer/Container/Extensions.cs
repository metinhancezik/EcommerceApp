using DataAccesLayer.Abstract;
using DataAccesLayer.EntityFramework;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Abstract;
using ServiceLayer.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLayer.Container
{
    public static class Extensions
    {


        public static void ContainerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserDetailService, UserDetailManager>();
            services.AddScoped<IUserDetail, EfUserDetail>();



        }


    }
}
