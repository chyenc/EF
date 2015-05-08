using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Data.Extensions;

namespace MyChy.Frame.Data
{
    /// <summary>
    /// 单元操作实现基类
    /// </summary>
    public abstract class UnitOfWorkContextBase : IUnitOfWorkContext
    {

        #region 属性

        /// <summary>
        /// 获取 当前使用的数据访问上下文
        /// </summary>
        protected abstract DbContext Context { get; }

        /// <summary>
        /// 获取 当前单元操作是否已被提交
        /// </summary>
        public bool IsCommitted { get; private set; }

        public DbContext DbContext { get { return Context; } }


        #endregion

        #region 方法
        /// <summary>
        /// 为指定的类型返回 System.Data.Entity.DbSet，这将允许对上下文中的给定实体执行 CRUD 操作。
        /// </summary>
        /// <typeparam name="TEntity">应为其返回一个集的实体类型。</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns>给定实体类型的 System.Data.Entity.DbSet 实例。</returns>
        public DbSet<TEntity> Set<TEntity, TKey>() where TEntity : EntityBase<TKey>
        {
            return Context.Set<TEntity>();
        }

        /// <summary>
        /// 注册一个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity">要注册的类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity">要注册的对象</param>
        public void RegisterNew<TEntity, TKey>(TEntity entity) where TEntity : EntityBase<TKey>
        {
            EntityState state = Context.Entry(entity).State;

            if (state == EntityState.Detached)
            {
                Context.Entry(entity).State = EntityState.Added;
            }

            IsCommitted = false;
        }

        /// <summary>
        /// 批量注册多个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity">要注册的类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entities">要注册的对象集合</param>
        public void RegisterNew<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : EntityBase<TKey>
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;

                foreach (var entity in entities)
                {
                    RegisterNew<TEntity, TKey>(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>
        ///     注册一个更改的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterModified<TEntity, TKey>(TEntity entity) where TEntity : EntityBase<TKey>
        {
           // Context.SaveChanges()
            Context.Update<TEntity, TKey>(entity);
            IsCommitted = false;
        }

        /// <summary>
        /// 使用指定的属性表达式指定注册更改的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity">要注册的类型</typeparam>
        /// <typeparam name="TKey">主键类型</typeparam>
        /// <param name="propertyExpression">属性表达式，包含要更新的实体属性</param>
        /// <param name="entity">附带新值的实体信息，必须包含主键</param>
        public void RegisterModified<TEntity, TKey>(Expression<Func<TEntity, object>> propertyExpression, TEntity entity) where TEntity : EntityBase<TKey>
        {
            Context.Update<TEntity, TKey>(propertyExpression, entity);
            IsCommitted = false;
        }

        /// <summary>
        ///   注册一个删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterDeleted<TEntity, TKey>(TEntity entity) where TEntity :EntityBase<TKey>
        {
            Context.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
        }

        /// <summary>
        ///   批量注册多个删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entities"> 要注册的对象集合 </param>
        public void RegisterDeleted<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : EntityBase<TKey>
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;

                foreach (TEntity entity in entities)
                {
                    RegisterDeleted<TEntity, TKey>(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }


        /// <summary>
        /// 提交当前单元操作的结果
        /// </summary>
        /// <param name="validateOnSaveEnabled">保存时是否自动验证跟踪实体</param>
        /// <returns></returns>
        public int Commit(bool validateOnSaveEnabled = true)
        {
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                int result = Context.SaveChanges(validateOnSaveEnabled);
                IsCommitted = true;
                return result;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException.InnerException is SqlException)
                {
                    var sqlEx = e.InnerException.InnerException as SqlException;
                    string msg = DataHelper.GetSqlExceptionMessage(sqlEx.Number);
                    //throw PublicHelper.ThrowDataAccessException("提交数据更新时发生异常：" + msg, sqlEx);
                }
                throw;
            }
        }

        /// <summary>
        /// 把当前单元回滚成未提交状态
        /// </summary>
        public void Rollback()
        {
            IsCommitted = false;
        }

        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }
            Context.Dispose();
        }


        ///<summary>
        ///执行Sql语句
        ///</summary>
        ///<param name="sqlstring">sp_Userinfos_deleteByID @ID 有几个参数，存储后面要带几个参数以逗号分隔</param>
        ///<param name="parameter">SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ID",id)};</param>
        ///<returns></returns>
        public int Execute(string sqlstring, object[] parameter)
        {
            //return this.EFContext.DbContext.Database.ExecuteSqlCommand(sqlstring, parameter);
            return Context.Database.ExecuteSqlCommand(sqlstring, parameter);
        }

        ///<summary>
        ///执行Sql语句
        ///</summary>
        ///<param name="sqlstring">delete UserInfoes  where id=@ID</param>
        ///<param name="parameter">SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ID",id)}; </param>
        ///<returns></returns>
        public IList<T> Execute<T>(string sqlstring, object[] parameter)
        {
            //return this.EFContext.DbContext.Database.ExecuteSqlCommand(sqlstring, parameter);
            return Context.Database.SqlQuery<T>(sqlstring, parameter).ToList();
        }


        #endregion


    }
}
