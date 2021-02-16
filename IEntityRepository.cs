﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetSatis.Entities.Context;

namespace NetSatis.Entities.Interfaces
{
    public interface IEntityRepository<TContext, TEntity>
         where TContext : DbContext, new()
        where TEntity : class, IEntity, new()
    {


        List<TEntity> GetAll(TContext context, Expression<Func<TEntity, bool>> filter = null);
        TEntity GetByFilter(TContext context, Expression<Func<TEntity, bool>> filter);

        bool AddOrUpdate(TContext context, TEntity entity);

        void Delete(TContext context, Expression<Func<TEntity, bool>> filter);

        void Save(TContext context);


    }
}
