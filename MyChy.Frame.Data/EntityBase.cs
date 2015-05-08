using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Data
{
    /// <summary>
    /// 可持久到数据库的领域模型的基类
    /// </summary>
    /// <typeparam name="TKey">主键的类型，如int，Guid等</typeparam>
    [Serializable]
    public abstract class EntityBase<TKey>
    {
        #region 构造函数

        /// <summary>
        /// 数据实体基类
        /// </summary>
        protected EntityBase()
        {
            IsDeleted = false;
            CreatorDate = DateTime.Now;
        }

        #endregion

        #region 属性

        [Key]
        public TKey Id { get; set; }

        /// <summary>
        /// 获取或设置 是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 获取或设置 添加时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime CreatorDate { get; set; }

        /// <summary>
        /// 获取或设置 添加人
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// 获取或设置 更新时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 获取或设置 更新人
        /// </summary>
        public int UpdaterId { get; set; }

        #endregion
    }
}
