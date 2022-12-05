using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VetAppointment.Application;
using VetAppointment.Infrastructure.Data;

#nullable disable
namespace VetAppointment.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DatabaseContext context;

        public Repository(DatabaseContext context) => this.context = context;

        public virtual T Add(T entity) => context.Add(entity).Entity;

        public virtual T Update(T entity) => context.Update(entity).Entity;

        public virtual T Get(Guid id) => context.Set<T>().Find(id);

        public virtual void Delete(T entity) => context.Set<T>().Remove(entity);

        public virtual IEnumerable<T> All() => context.Set<T>().ToList();

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => context.Set<T>().Where(predicate).ToList();

        public void SaveChanges() => context.Save();
    }
}
