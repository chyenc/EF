using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MyChy.Frame.Common.Helper
{
    public class logHelper
    {
        private static readonly ILog ILog = null;

        static logHelper()
        {
            ILog = LogManager.GetLogger(typeof(logHelper));
        }

        /// <summary>
        /// 写日志，默认级别是 LogLevel.Error
        /// </summary>
        /// <param name="message">写日志的内容</param>
        public static void LogError(string message)
        {
            ILog.Error(message);
        }

        /// <summary>
        /// 写日志，默认级别是 LogLevel.Error
        /// </summary>
        /// <param name="message">写日志的内容</param>
        public static void LogInfo(string message)
        {
            ILog.Info(message);
        }

        /// <summary>
        /// 写日志，默认日志级别是 LogLevel.Error。会根据异常的类型来判断是否发送通知邮件
        /// </summary>
        /// <param name="exception">异常信息</param>
        public static void Log(Exception exception)
        {
            ILog.Error(exception);
        }
    }
}
