using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.Helper
{
    /// <summary>
    /// 内部验证帮助
    /// </summary>
    internal sealed class SafeCheck
    {
        #region 图片验证

        /// <summary>
        /// 判断是否为图片
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static bool IsJPGstr(string Files)
        {
            string namex = ".jpg|.gif|.jpeg|.png|.bmp";
            return IsJPGstr(Files, namex);
        }
        public static bool IsJPGstr(string Files, string ext)
        {
            return IsStr(Files, ext);
        }

        #endregion

        #region Flash  验证

        /// <summary>
        /// 判断是否为其他文件
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static bool IsOther(string Files, string namex)
        {
            string name = ".txt|.zip|.rar|.doc|.docx|.xlsx|.xls|.ppt|.pdf|.inf";
            bool result = false;
            if (string.IsNullOrEmpty(namex))
            {
                result = IsStr(Files, name);
            }
            else
            {
                result = IsStr(Files, namex);
            }
            return result;

        }

        #endregion

        #region 文件验证

        /// <summary>
        /// 判断是否为Falsh
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static bool IsSwfstr(string Files)
        {
            string namex = ".swf|.fla";
            return IsStr(Files, namex);

        }

        #endregion


        #region 私有方法
        /// <summary>
        /// 文件验证
        /// </summary>
        /// <param name="Files">文件名</param>
        /// <param name="ext">扩展名</param>
        /// <returns></returns>
        private static bool IsStr(string Files, string ext)
        {
            try
            {
                string namex = ext;
                string ex = (IOFiles.GetExtension(Files)).ToLower();
                foreach (string extname in namex.ToString().Split('|'))
                {
                    if (ex == extname)
                    {
                        return true;

                    }
                }
                return false;
            }
            catch
            {
                return false;
            }

        }


        #endregion


        #region 判断是否为物理地址

        public static bool IsMap(string Files)
        {
            Files = Files.Replace("//", ":");
            if (Files.IndexOf(':') > 0)
            {
                return true;
            }
            return false;

        }

        #endregion
    }
}
