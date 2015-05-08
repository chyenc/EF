using System;
using System.Web;
using MyChy.Frame.Common.MvcHelper;

namespace MyChy.Frame.Common.Helper
{
    public class CookieHelper
    {
        public CookieHelper()
        {

        }

        private static readonly string Des3Key = WebConfig.AppSettingsName<string>("CookieHelperKey", "dtvb^*3e");

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        /// <param name="domainname">如果为空，则设置当前域名</param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static bool SetCookie(string cookiename, string cookievalue, string domainname, int hour, int minute)
        {
            if (cookievalue == null) return false;
            //设定cookie 过期时间.
            DateTime dtExpiry = DateTime.Now.AddHours(hour).AddMinutes(minute);
            var httpCookie = HttpContext.Current.Response.Cookies[cookiename];
            if (httpCookie == null) return false;
            HttpCookie sessionCookie = null;

            //对 SessionId 进行备份.
            if (HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null)
            {
                string sesssionId = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString();
                sessionCookie = new HttpCookie("ASP.NET_SessionId") { Value = sesssionId };
            }

            httpCookie.Expires = dtExpiry;
            //设定cookie 域名.
            if (domainname.Length == 0)
            {
                string domain = string.Empty;
                if (HttpContext.Current.Request.Params["HTTP_HOST"] != null)
                {
                    //domain = "www.elong.com";
                    domain = HttpContext.Current.Request.Params["HTTP_HOST"].ToString();
                }

                if (domain.IndexOf(".", System.StringComparison.Ordinal) > -1)
                {
                    int index = domain.IndexOf(".", System.StringComparison.Ordinal);
                    domain = domain.Substring(index + 1);
                    httpCookie.Domain = domain;

                }
            }
            else
            {
                httpCookie.Domain = domainname;
            }

            httpCookie.Value = cookievalue;

            //如果cookie总数超过20 个, 重写ASP.NET_SessionId, 以防Session 丢失.
            if (HttpContext.Current.Request.Cookies.Count <= 20 || sessionCookie == null) return true;
            if (sessionCookie.Value == string.Empty) return true;
            HttpContext.Current.Response.Cookies.Remove("ASP.NET_SessionId");
            HttpContext.Current.Response.Cookies.Add(sessionCookie);
            return true;
        }

        public static bool SetCookie(string cookiename, string cookievalue, string domainname)
        {
            if (cookievalue == null) return false;
            var httpCookie = HttpContext.Current.Response.Cookies[cookiename];
            if (httpCookie != null)
            {
                if (domainname.Length == 0)
                {
                    string domain = string.Empty;
                    if (HttpContext.Current.Request.Params["HTTP_HOST"] != null)
                    {
                        //domain = "www.elong.com";
                        domain = HttpContext.Current.Request.Params["HTTP_HOST"].ToString();
                    }

                    if (domain.IndexOf(".", System.StringComparison.Ordinal) > -1)
                    {
                        int index = domain.IndexOf(".", System.StringComparison.Ordinal);
                        domain = domain.Substring(index + 1);
                        httpCookie.Domain = domain;
                    }
                }
                else
                {
                    httpCookie.Domain = domainname;
                }
                httpCookie.Value = cookievalue;
            }
            return true;

        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookiekey"></param>
        /// <returns></returns>
        public static string GetCookie(string cookiekey)
        {
            string cookyval;
            try
            {
                if (HttpContext.Current.Request.Cookies[cookiekey] == null)
                {
                    return "";
                }
                cookyval = HttpContext.Current.Request.Cookies[cookiekey].Value;
            }
            catch
            {
                return "";
            }
            return cookyval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        /// <param name="domainname"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static bool SetCookie3Des(string cookiename, string cookievalue, string domainname, int hour, int minute)
        {
            cookievalue = SafeSecurity.EncryptDES(cookievalue, Des3Key);
            return SetCookie(cookiename, cookievalue, domainname, hour, minute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookiekey"></param>
        /// <returns></returns>
        public static string GetCookie3Des(string cookiekey)
        {
            string cookyval = GetCookie(cookiekey);
            if (!String.IsNullOrEmpty(cookiekey))
            {
                cookyval = SafeSecurity.DecryptDES(cookyval, Des3Key);
            }
            return cookyval;
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="cookiekey"></param>
        public static void RemoveCookie(string cookiekey)
        {
            CookieHelper.SetCookie(cookiekey, "", "", -1, 0);
        }
    }
}
