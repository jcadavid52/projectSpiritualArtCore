using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IServices<Entity> where Entity : class
    {
        Task<IEnumerable<Entity>> GetAll();
        Task<IEnumerable<Entity>> Filters(Expression<Func<Entity,bool>> expression);
    }
}
