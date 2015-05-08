using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyChy.Frame.Common.Extensions
{
    public static class ObjectExtension
    {
        private static readonly Type ValueTypeType = typeof(ValueType);

        /// <summary>
        /// 将字符转换成自己的类型
        /// </summary>
        /// <param name="val">System.String</param>
        /// <returns>如果转换失败将返回 T 的默认值</returns>
        public static T ToT<T>(this object val)
        {
            return val != null ? val.ToT<T>(default(T)) : default(T);
        }

        /// <summary>
        /// 当前对象转换成特定类型，如果转换失败或者对象为null，返回defaultValue。
        /// 如果传入的是可空类型：会试着转换成其真正类型后返回。
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">原类型对象</param>
        /// <param name="defaultValue">转换出错或者对象为空的时候的默认返回值</param>
        /// <returns>转换后的值</returns>
        public static T ToT<T>(this object value, T defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else if (value is T)
            {
                return (T)value;
            }
            try
            {
                var typ = typeof(T);
                if (typ.BaseType != ObjectExtension.ValueTypeType || !typ.IsGenericType)
                    return (T) Convert.ChangeType(value, typeof (T));
                var typs = typ.GetGenericArguments();
                return (T)Convert.ChangeType(value, typs[0]);
            }
            catch
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// 根据类型 名称 获取context值
        /// </summary>
        /// <param name="ty">类型</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetValueByType(Type ty, object value)
        {
            if (value == null) return null;
            try
            {
                var objvalue= string.Format("\"{0}\"", value);
                var result = JsonConvert.DeserializeObject(objvalue, ty);
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
