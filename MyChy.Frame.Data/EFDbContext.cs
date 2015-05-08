using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Data
{
    [Export("EF", typeof(DbContext))]
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("default")
        {
        }

        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public EFDbContext(DbConnection existingConnection)
            : base(existingConnection, true)
        {
        }

        [ImportMany(typeof(IEntityMapper))]
        public IEnumerable<IEntityMapper> EntityMappers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            if (EntityMappers == null)
            {
                return;
            }

            foreach (var mapper in EntityMappers)
            {
                mapper.RegistTo(modelBuilder.Configurations);
            }
        }
    }
}
