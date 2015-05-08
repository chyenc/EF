using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyChy.Frame.Common.Extensions;

namespace MyChy.Frame.Common.Helper
{
    public class SafaCookieHelper
    {
        private static IList<string> _codeList;
        private static readonly object Obj = new object();

        private const string DesKey = "ct$we3@o";

        public static void WriteCode(string val, string cookieName, string cookieKey)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            DateTime time = DateTime.Now.AddMinutes(2);
            string valx = string.Format("{0}_{1}_{2}_{3}",
                SafeSecurity.MD5Encrypt(time.Ticks.ToString()) + time.Ticks, ip, val, time);
            string des = SafeSecurity.EncryptDES(valx, DesKey);
            CookieHelper.SetCookie(cookieName, cookieKey, des);
        }

        public static string ReadCode(string cookieName)
        {
            string result = string.Empty;
            string des = CookieHelper.GetCookie(cookieName);
            if (!string.IsNullOrEmpty(des))
            {
                string val = SafeSecurity.DecryptDES(des, DesKey);
                string[] vallist = val.Split('_');
                if (vallist.Length == 4)
                {
                    string ip = HttpContext.Current.Request.UserHostAddress;
                    var time = vallist[3].ToT<DateTime>(DateTime.Now.AddDays(-1));
                    if (ip == vallist[1] && time > DateTime.Now)
                    {
                        result = vallist[2];
                    }
                }
            }
            CookieHelper.RemoveCookie(cookieName);
            return result;
        }


        /// <summary>
        /// 放在用户一个Cookie重复提交
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool CheckCodeList(string code)
        {
            if (_codeList == null)
            {
                _codeList = new List<string>(500);
            }
            if (_codeList.Contains(code)) return true;
            lock (Obj)
            {
                _codeList.Add(code);
                if (_codeList.Count <= 499) return false;
                for (int i = 0, count = _codeList.Count - 400; i < count; i++)
                {
                    _codeList.RemoveAt(i);
                }
            }
            return false;
        }
    }
}
