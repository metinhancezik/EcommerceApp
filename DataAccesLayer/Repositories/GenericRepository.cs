using DataAccesLayer.Concrete;
using DataAccesLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccesLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly IServiceProvider _serviceProvider;

        public GenericRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private Context CreateContext()
        {
            return _serviceProvider.GetRequiredService<Context>();
        }

        public void Delete(T t)
        {
            using var c = CreateContext();
            c.Remove(t);
            c.SaveChanges();
        }

        public T GetByID(int id)
        {
            using var c = CreateContext();
            return c.Set<T>().Find(id);
        }

        public List<T> GetListAll()
        {
            using var c = CreateContext();
            return c.Set<T>().ToList();
        }

        public void Insert(T t)
        {
            using var c = CreateContext();
            c.Add(t);
            c.SaveChanges();
        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            using var c = CreateContext();
            return c.Set<T>().Where(filter).ToList();
        }

        public void Update(T t)
        {
            using var c = CreateContext();
            c.Update(t);
            c.SaveChanges();
        }
    }
}