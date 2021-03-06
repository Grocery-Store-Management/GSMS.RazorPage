using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly GsmsContext context;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(GsmsContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(string id)
        {
            T entity = GetAsync(id).Result;
            if (entity == null)
            {
                throw new Exception("Entity does not exist!!");
            }
            Delete(entity);
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync(string sqlQuery)
        {
            return await dbSet.FromSqlRaw(sqlQuery).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] otherEntities)
        {
            IQueryable<T> entities = null;
            foreach (string other in otherEntities)
            {
                if (entities == null)
                {
                    entities = dbSet.Include(other);
                }
                else
                {
                    entities = entities.Include(other);
                }
            }
            return await entities.ToListAsync();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}
