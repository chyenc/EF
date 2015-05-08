using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Data;

namespace MyChy.Core.Data.Migrations
{
    /// <summary>
    /// 数据模型相关配置及数据库数据初始化
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }
        protected void Seed_new(EFDbContext context)
        {

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="context">EFDbContext的对象</param>
        protected override void Seed(EFDbContext context)
        {
            try
            {
                context.Configuration.ValidateOnSaveEnabled = false;
                ModuleSeed(context);
                context.Configuration.ValidateOnSaveEnabled = true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        //模块初始化
        private void ModuleSeed(EFDbContext context)
        {
            #region 模块初始化
          
            #endregion

            //DbSet<Module> moduleSet = context.Set<Module>();
            //moduleSet.AddOrUpdate(m => new { m.ModuleCode, m.ModuleName, m.Url }, modules.ToArray());
        }
    }
}
