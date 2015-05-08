using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Common.Extensions;

namespace MyChy.Frame.Common.MvcHelper
{
    public static class WebConfig
    {
        /// <summary>
        /// 根据配置节key获取连接字符串
        /// </summary>
        public static T AppSettingsName<T>(string strSettingName, T defaultValue)
        {
            return ConfigurationManager.AppSettings[strSettingName].ToT<T>(defaultValue);
        }

        /// <summary>
        /// 根据配置节key获取连接字符串
        /// </summary>
        public static T AppSettingsName<T>(string strSettingName)
        {
            return ConfigurationManager.AppSettings[strSettingName].ToT<T>();
        }

    }
}
