using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MgAPI.Data.Entities;
using MgAPI.Data.Interfaces;

namespace MgAPI.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly Context context;

        public BaseRepository(Context context)
        {
            this.context = context;
        }
        public void Create(T item)
        {
            context.Set<T>().Add(item);
            context.SaveChanges();
        }
        public T Read(string id)
        {
            return context.Set<T>().Find(id);

        }

        public T Read(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().SingleOrDefault(predicate);
        }
        public ICollection<T> ReadAll()
        {
            return context.Set<T>().ToList();
        }

        public ICollection<T> ReadAll(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate).ToList();
        }

        public void Update(T item)
        {
            T itemToUpdate = context.Set<T>().Find(item.ID);
            context.Entry(itemToUpdate).CurrentValues.SetValues(item);

            context.SaveChanges();
        }

        public void Delete(string key)
        {
            T itemToRemove = context.Set<T>().FirstOrDefault(x => x.ID == key);
            context.Set<T>().Remove(itemToRemove);

            context.SaveChanges();
        }
    }
}
