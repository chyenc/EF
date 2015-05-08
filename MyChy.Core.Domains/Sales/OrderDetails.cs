using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Data;

namespace MyChy.Core.Domains.Sales
{
    public class OrderDetails : EntityBase<int>
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        [ForeignKey("Order")]
        public int Order_Id { get; set; }


        /// <summary>
        /// 状态 0 未完成 1完成 用于虚拟物品 单独发货
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 0 可以购买 1暂停 2下架
        /// </summary>
        public int IsBuy { get; set; }

        /// <summary>
        /// 是否快递行 没明白
        /// 1-快递行；0-非快递行
        /// </summary>
        public bool IsExpress { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int TagColor { get; set; }

        /// <summary>
        /// 颜色名称
        /// </summary>
        [StringLength(11)]
        public string TagColorTitle { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品价格(单价)
        /// </summary>
        public double MemberPrice { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        public double TotalPrice { get; set; }
    }
}
