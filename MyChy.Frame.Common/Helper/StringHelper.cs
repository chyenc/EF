using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Security.Application;
using Newtonsoft.Json;

namespace MyChy.Frame.Common.Helper
{
    public static class StringHelper
    {
        private static readonly char[] Constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// 生成随机数字码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int length)
        {
            var guid = Guid.NewGuid();
            var newRandom = new System.Text.StringBuilder(length);
            var rd = new Random(guid.GetHashCode());
            for (var i = 0; i < length; i++)
            {
                newRandom.Append(Constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

        public static string GenerateRandomCode(int num)
        {
            var guid = Guid.NewGuid();
            var random = new Random(guid.GetHashCode());
            var newRandom = new System.Text.StringBuilder(num);
            for (var i = 0; i < num; i++)
            {
                var number = random.Next();

                char code;
                if (number % 2 == 0)
                    code = (char)('1' + (char)(number % 9));
                else
                    code = (char)('A' + (char)(number % 26));

                newRandom.Append(code);
            }
            return newRandom.ToString();

        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string value)
        {
             return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 处理页面特殊字符串
        /// </summary>
        /// <param name="htmlEncode"></param>
        /// <returns></returns>
        public static string HandleSpecialHtmlTag(string htmlEncode)
        {
            if (!string.IsNullOrEmpty(htmlEncode))
            {
                htmlEncode = htmlEncode.Replace("<br />", "&lt;br/&gt;").Replace("<img", "&lt;img");
                htmlEncode = htmlEncode.Replace("<table", "&lt;table").Replace("</table", "&lt;/table");
                htmlEncode = htmlEncode.Replace("<tr", "&lt;tr").Replace("<td", "&lt;td");
                htmlEncode = htmlEncode.Replace("</tr", "&lt;/tr").Replace("</td", "&lt;/td");
                htmlEncode = htmlEncode.Replace("<span", "&lt;span").Replace("</span", "&lt;/span");
                htmlEncode = htmlEncode.Replace("<ul", "&lt;ul").Replace("</ul", "&lt;/ul");
                htmlEncode = Sanitizer.GetSafeHtmlFragment(htmlEncode);
                //htmlEncode = htmlEncode.Replace("[br/]", "<br />").Replace("[img", "<img");
            }
            return htmlEncode;
        }

        /// <summary>
        /// 解码html
        /// </summary>
        /// <param name="htmlEncode"></param>
        /// <returns></returns>
        public static string HtmlDecode(string htmlEncode)
        {
            if (string.IsNullOrEmpty(htmlEncode))
            {
                return string.Empty;
            }

            return htmlEncode.Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&lt;", "<");
        }

        /// <summary>
        /// 解码html
        /// </summary>
        /// <param name="htmlEncode"></param>
        /// <returns></returns>
        public static string HtmlDecodeTextarea(string htmlEncode)
        {
            if (string.IsNullOrEmpty(htmlEncode))
            {
                return string.Empty;
            }
            return htmlEncode.Replace(">", "&gt;").Replace("<", "&lt;").Replace("\r", "<br/>");

        }
    }
}
