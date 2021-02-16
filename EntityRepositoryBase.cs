using NetSatis.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSatis.Entities.Repository;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;
using FluentValidation;
using NetSatis.Entities.Tools;

namespace NetSatis.Entities.Repository
{
    public class EntityRepositoryBase<TContext, TEntity,TValidator> : IEntityRepository<TContext, TEntity>
            where TContext : DbContext, new()
          where TEntity : class, IEntity, new()
         where TValidator: IValidator, new()
    {

       public  List<TEntity>  GetAll(TContext context, Expression<Func<TEntity, bool>> filter=null)
        {
            return filter == null ? context.Set<TEntity>().ToList()
                 : context.Set<TEntity>().Where(filter).ToList();
        }

        public TEntity GetByFilter(TContext context, Expression<Func<TEntity, bool>> filter )
        {
            return context.Set<TEntity>().SingleOrDefault(filter);
                 
        }


       

        public void Delete(TContext context, Expression<Func<TEntity, bool>> filter)
        {
            context.Set<TEntity>().RemoveRange(context.Set<TEntity>().Where(filter));
        }

        public void Save(TContext context)
        {

            context.SaveChanges();
        }

        public bool AddOrUpdate(TContext context, TEntity entity)
        {
            bool result = false;
            TValidator validator = new TValidator();
            var validationResult = ValidatorTool.Validate(validator, entity);
            if (validationResult)
            {
                context.Set<TEntity>().AddOrUpdate(entity);
                result = true;
            }

            return result;

        }
    }
}
