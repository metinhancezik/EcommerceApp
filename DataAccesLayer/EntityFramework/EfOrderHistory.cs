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
    public class EfOrderHistory : GenericRepository<OrderHistory>, IOrderHistory
    {


        // Sipariş geçmişini getir
        public async Task<List<OrderHistory>> GetOrderHistoryByOrderIdAsync(long orderInformationId)
        {
            using var context = new Context();
            return await context.Set<OrderHistory>()
                .Include(oh => oh.State)
                .Where(oh => oh.OrderInformationId == orderInformationId)
                .OrderByDescending(oh => oh.StatusUpdatedTime)
                .ToListAsync();
        }

        // Son durumu getir
        public async Task<OrderHistory> GetLastStatusAsync(long orderInformationId)
        {
            using var context = new Context();
            return await context.Set<OrderHistory>()
                .Include(oh => oh.State)
                .Where(oh => oh.OrderInformationId == orderInformationId)
                .OrderByDescending(oh => oh.StatusUpdatedTime)
                .FirstOrDefaultAsync();
        }

        // Belirli bir tarihteki sipariş geçmişlerini getir
        public async Task<List<OrderHistory>> GetOrderHistoryByDateAsync(DateTime date)
        {
            using var context = new Context();
            return await context.Set<OrderHistory>()
                .Include(oh => oh.State)
                .Include(oh => oh.OrderInformation)
                .Where(oh => oh.StatusUpdatedTime.Date == date.Date)
                .OrderByDescending(oh => oh.StatusUpdatedTime)
                .ToListAsync();
        }
    }
}
