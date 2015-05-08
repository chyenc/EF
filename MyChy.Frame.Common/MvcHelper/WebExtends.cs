using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.MvcHelper
{
    public static class WebExtends
    {
        #region 转换成URL参数
        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToQueryString(this IDictionary<object, object> dictionary)
        {
            var sb = new StringBuilder();
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.Append(key + "=" + value + "&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }

        public static string ToQueryString(this IEnumerable<object> list, string key)
        {
            var sb = new StringBuilder();
            foreach (var val in list)
            {
                if (val != null)
                {
                    sb.Append(key + "=" + Uri.EscapeDataString(val.ToString()) + "&");
                }
            }
            return sb.ToString().TrimEnd('&').Substring(key.Length + 1);
        }
        #endregion
    }
}
