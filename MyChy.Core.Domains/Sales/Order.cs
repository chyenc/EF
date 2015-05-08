using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Data;

namespace MyChy.Core.Domains.Sales
{
    [Description("订单信息")]
    public class Order : EntityBase<int>
    {

        /// <summary>
        /// 主单
        /// </summary>
        public int MainOrder { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(11)] 
        public string Title { get; set; }


        public OrderStatus Status { get; set; }

        /// <summary>
        /// 版本信息 放心多线程操作
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
