using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Northwind.Contracts;
using Northwind.Entities.Contexts;

namespace Northwind.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        // inject repositoryContext for connection to db
        protected RepositoryContext repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public void Create(T entity) => repositoryContext.Set<T>().Add(entity);

        public void Delete(T entity)=> repositoryContext.Set<T>().Remove(entity);

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expressions, bool trackChanges) =>
            !trackChanges ? repositoryContext.Set<T>().Where(expressions).AsNoTracking()
                : repositoryContext.Set<T>().Where(expressions);


        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? repositoryContext.Set<T>().AsNoTracking() : repositoryContext.Set<T>();


        public void Update(T entity) => repositoryContext.Set<T>().Update(entity);
    }
}
