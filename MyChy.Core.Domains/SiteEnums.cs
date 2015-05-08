using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Core.Domains
{
    /// <summary>
    /// 订单状态
    /// </summary>
    [Description("订单状态")]
    public enum OrderStatus
    {
        /// <summary>
        /// 等待确认
        /// </summary>
        //[Description("等待确认")]
        //WaitForConfirm = 1,

        /// <summary>
        /// 等待付款
        /// </summary>
        [Description("等待付款")]
        WaitForPay = 2,

        /// <summary>
        /// 等待拣货
        /// </summary>
        [Description("等待拣货")]
        WaitPicking = 8,

        /// <summary>
        /// 等待发货
        /// </summary>
        [Description("等待发货")]
        WaitForDeliver = 3,
        /// <summary>
        /// 等待收货
        /// </summary>
        [Description("等待收货")]
        WaitForReceive = 4,
        /// <summary>
        /// 订单完成
        /// </summary>
        [Description("订单完成")]
        Finish = 5,
        /// <summary>
        /// 退货申请
        /// </summary>
        [Description("退货申请")]
        Return = 6,
        /// <summary>
        /// 订单关闭
        /// </summary>
        [Description("订单关闭")]
        Close = 7,

        /// <summary>
        /// 订单退款
        /// </summary>
        [Description("订单退款")]
        Returns = 9,

        /// <summary>
        /// 申请撤单
        /// </summary>
        [Description("申请撤单")]
        Cancellation = 10,

    }
}
