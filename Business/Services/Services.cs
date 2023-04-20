using Business.Interfaces;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class Services<Entity> : IServices<Entity> where Entity : class
    {
        private readonly SpiritualDbContext _dbContext;

        public Services(SpiritualDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<Entity> EntitySet
        {
            get { return _dbContext.Set<Entity>(); }
        }

        public async Task<IEnumerable<Entity>> GetAll()
        {
            var lstEntity = await EntitySet.ToListAsync();

            return lstEntity;
        }

        public async Task<IEnumerable<Entity>> Filters(Expression<Func<Entity, bool>> expression)
        {
            return await EntitySet.AsNoTracking().Where(expression).ToListAsync(); 
        }
    }
}
