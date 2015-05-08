using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Data
{
    /// <summary>
    /// 数据单元操作类
    /// </summary>
    [Export(typeof(IUnitOfWork))]
    public class EFUnitOfWorkContext : UnitOfWorkContextBase
    {
        protected override DbContext Context
        {
            get { return this.EFDbContext.Value; }
        }

        [Import("EF", typeof(DbContext))]
        private Lazy<EFDbContext> EFDbContext { get; set; }
    }
}
