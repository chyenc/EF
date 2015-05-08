using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using MyChy.Frame.Common.Helper;

namespace MyChy.Frame.Common.MvcHelper
{
    public class WebHelper
    {

        const string SUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        const string SContentType = "application/x-www-form-urlencoded";
        const string SRequestEncoding = "ascii";
        const string SResponseEncoding = "gb2312";


        /// <summary>
        /// 发送Web请求返回结果
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postDataStr">参数 例如:arg1=a&arg2=b</param>
        /// <returns></returns>
        public static string WebFormPost(string url, string postDataStr)
        {
            var retString = string.Empty;
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                //上面的http头看情况而定，但是下面俩必须加  
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.Timeout = 100000;

                var encoding = Encoding.UTF8;//根据网站的编码自定义  
                byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
                request.ContentLength = postData.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                //requestStream.Close();  

                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
                    if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
                    {
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }

                    var streamReader = new StreamReader(responseStream, encoding);
                    retString = streamReader.ReadToEnd();

                    streamReader.Close();
                    responseStream.Close();
                }

            }
            catch (Exception e)
            {
                logHelper.Log(e);
            }

            return retString;
        }

        /// <summary>
        /// 发送Web请求返回结果
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postDataStr">参数 例如:arg1=a&arg2=b</param>
        /// <returns></returns>
        public static string WebFormGet(string url, string postDataStr)
        {
            var retString = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(postDataStr))
                {
                    url = url + "?" + postDataStr;
                }
                var request = WebRequest.Create(url) as HttpWebRequest;
                //上面的http头看情况而定，但是下面俩必须加  
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "Get";
                request.Timeout = 100000;

                var encoding = Encoding.UTF8;//根据网站的编码自定义  
                //byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
                //request.ContentLength = postData.Length;
                //Stream requestStream = request.GetRequestStream();
                //requestStream.Write(postData, 0, postData.Length);
                //requestStream.Close();  

                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
                    if (response.Headers["Content-Encoding"] != null &&
                        response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
                    {
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }

                    var streamReader = new StreamReader(responseStream, encoding);
                    retString = streamReader.ReadToEnd();

                    streamReader.Close();
                    responseStream.Close();
                }
            }
            catch (Exception e)
            {
                logHelper.Log(e);
            }

            return retString;
        }


        #region Post With Pic

        public static string WebFormPostPic(string url, IDictionary<object, object> param, string filePath)
        {
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            var wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();
            string responseStr = null;

            const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in param.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, param[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "pic", filePath, "text/plain");
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseStr = reader2.ReadToEnd();
                //logger.Debug(string.Format("File uploaded, server response is: {0}", responseStr));
            }
            catch (Exception ex)
            {
                //logger.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
            return responseStr;
        }

        /// <summary>
        /// HTTP POST方式请求数据(带图片)
        /// </summary>
        /// <param name="url">URL</param>        
        /// <param name="param">POST的数据</param>
        /// <param name="fileByte">图片</param>
        /// <returns></returns>
        public static string WebFormPostPic(string url, IDictionary<object, object> param, byte[] fileByte)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();
            string responseStr = null;

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in param.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, param[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "pic", fileByte, "text/plain");//image/jpeg
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            rs.Write(fileByte, 0, fileByte.Length);

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseStr = reader2.ReadToEnd();
                //logger.Error(string.Format("File uploaded, server response is: {0}", responseStr));
            }
            catch (Exception ex)
            {
                //logger.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
            return responseStr;
        }

        #endregion

        /// <summary>
        /// 获取当前用户IP
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            string result;
            try
            {
                // 如果使用代理，获取真实IP 
                result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "" ?
                    System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] :
                    System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(result))
                    result = System.Web.HttpContext.Current.Request.UserHostAddress;
                return result;

            }
            catch (Exception)
            {
                result = "0.0.0.0";
                //throw;
            }
            return result;
        }

        /// <summary>
        /// 获取固定位置参数值
        /// </summary>
        /// <param name="length"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetParam(int length, string url)
        {
            var result = string.Empty;
            var arr = url.Split('&');
            if (arr.Length > 0 && arr.Length > length)
            {
                result = arr[length].Split('=')[1];
            }
            return result;
        }

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] RedFile(string file)
        {
            var filePath = IOFiles.GetFileMapPath(file);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Close();
            return buffer;
        }


        /// <summary>
        /// 判断是否邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            const string strExp = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            var r = new Regex(strExp);
            var m = r.Match(email);
            return m.Success;
        }
    }
}
