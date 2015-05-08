using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.Extensions
{
    /// <summary>
    ///     枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this Enum obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string obj) where T : struct
        {
            if (string.IsNullOrEmpty(obj))
            {
                return default(T);
            }
            try
            {
                return (T)Enum.Parse(typeof(T), obj, true);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDescription(this Type type, int? id)
        {
            var values = from Enum e in Enum.GetValues(type)
                         select new { id = e.ToInt(), name = e.ToDescription() };

            if (!id.HasValue) id = 0;

            return values.ToList().Find(c => c.id == id).name;
        }
        /// <summary>
        ///     获取枚举项的Description特性的描述文字
        /// </summary>
        /// <param name="enumeration"> </param>
        /// <returns> </returns>
        public static string ToDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] members = type.GetMember(enumeration.ToT<string>());
            if (members.Length > 0)
            {
                return members[0].ToDescription();
            }
            return enumeration.ToT<string>();
        }
    }
}
