using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace CourierConnect.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db) 
        {
            _db = db;
            this.dbSet = _db.Set<T>();

            _db.Inquiries.Include(u => u.destinationAddress).Include(u => u.sourceAddress).Include(u => u.package);
            _db.Offers.Include(u => u.inquiry);
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            //IQueryable<T> query = dbSet;
            //query = query.Where(filter);
            //return query.FirstOrDefault();

            return FindAll(filter, includeProperties).FirstOrDefault();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();

        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            //IQueryable<T> query = dbSet;
            //return query.ToList();

            return FindAll(u => true, includeProperties);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
