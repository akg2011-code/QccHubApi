using Microsoft.EntityFrameworkCore;
using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QccHub.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public Task<List<T>> GetAllAsync()
        {
            return context.Set<T>().ToListAsync();
        }

        public Task<T> GetByIdAsync(int id)
        {
            return context.Set<T>().FirstOrDefaultAsync(x => x.ID == id);
        }


        public T Add(T entity)
        {
            context.Set<T>().Add(entity);
            return entity;
        }
        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }
    }
}
