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

            services.AddScoped<IUserAuthService, UserAuthManager>();
            services.AddScoped<IUserAuth, EfUserAuth>();

            services.AddScoped<IAuthTokensService, AuthTokensManager>();
            services.AddScoped<IAuthTokens, EfAuthTokens>();

            services.AddScoped<ICountryService, CountryManager>();
            services.AddScoped<ICountry, EfCountry>();

            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<ICart, EfCart>();

            services.AddScoped<ICityService, CityManager>();
            services.AddScoped<ICity, EfCity>();

            services.AddScoped<IDistrictService, DistrictManager>();
            services.AddScoped<IDistrict, EfDistrict>();

            services.AddScoped<INeighborhoodService, NeighborhoodManager>();
            services.AddScoped<INeighborhood, EfNeighborhood>();

            services.AddScoped<IOrderHistoryService, OrderHistoryManager>();
            services.AddScoped<IOrderHistory, EfOrderHistory>();

            services.AddScoped<IOrderInformationsService, OrderInformationsManager>();
            services.AddScoped<IOrderInformations, EfOrderInformations>();

            services.AddScoped<IOrderItemsService, OrderItemsManager>();
            services.AddScoped<IOrderItems, EfOrderItems>();

            services.AddScoped<IOrderStateService, OrderStateManager>();
            services.AddScoped<IOrderState, EfOrderState>();

            services.AddScoped<IOrderStatusService, OrderStatusManager>();
            services.AddScoped<IOrderStatus, EfOrderStatus>();

            services.AddScoped<IPasswordResetsService, PasswordResetsManager>();
            services.AddScoped<IPasswordResets, EfPasswordResets>();

            services.AddScoped<IProductsService, ProductsManager>();
            services.AddScoped<IProducts, EfProducts>();
 
            services.AddScoped<IVendorsService, VendorsManager>();
            services.AddScoped<IVendors, EfVendors>();
        }


    }
}
