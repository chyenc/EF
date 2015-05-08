using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyChy.Frame.Common.Extensions;

namespace MyChy.Frame.Common.DataHelper
{
    public class ModelHelper
    {
        /// <summary>
        /// table 自动转换成类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="da"></param>
        /// <returns></returns>
        public static T GetModelByTable<T>(DataTable da)
        {
            if (da.Rows.Count == 0) return default(T);

            IList<T> result = GetListModelByTable<T>(da);
            if (result != null && result.Count > 0)
            {
                return result.ToList<T>()[0];
            }
            else
            {
                return default(T);
            }
        }


        /// <summary>
        /// table 自动转换成类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="da"></param>
        /// <returns></returns>
        public static IList<T> GetListModelByTable<T>(DataTable da)
        {
            IList<T> result = new List<T>();
            if (da == null)
            {
                return result;
            }

            Type t = typeof(T);
            object model = Activator.CreateInstance(t);
            PropertyDescriptorCollection col = TypeDescriptor.GetProperties(model);
            HashSet<string> list = new HashSet<string>();

            foreach (PropertyDescriptor item in col)
            {
                if (da.Columns.Contains(item.Name))
                {
                    list.Add(item.Name);
                }
            }

            foreach (DataRow dr in da.Rows)
            {
                model = Activator.CreateInstance(t);
                foreach (PropertyDescriptor item in col)
                {
                    if (list.Contains(item.Name))
                    {
                        object value = ObjectExtension.GetValueByType(item.PropertyType, dr[item.Name]);
                        if (value != null)
                        {
                            item.SetValue(model, value);
                        }
                    }
                }
                if (model != null)
                {
                    result.Add((T)model);
                }
            }

            return result;
        }

        /// <summary>
        /// 类 自动转换成 table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable GetTableByListModel<T>(IList<T> list)
        {
            DataTable result = new DataTable();
            if (list == null)
            {
                return result;
            }
            Type t = typeof(T);
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo item in pi)
            {
                result.Columns.Add(new DataColumn(item.Name, item.PropertyType));
            }
            var newRow = result.NewRow();
            foreach (var i in list)
            {
                newRow = result.NewRow();
                for (int j = 0; j < result.Columns.Count; j++)
                {
                    newRow[j] = t.InvokeMember(result.Columns[j].ColumnName, BindingFlags.GetProperty,
                        null, i, new object[] { });
                }
                result.Rows.Add(newRow);
            }
            return result;
        }


        public static IList<T> GetListModelByTablebyDesc<T>(DataTable da)
        {
            IList<T> result = new List<T>();
            if (da == null)
            {
                return result;
            }

            Type t = typeof(T);
            object model = Activator.CreateInstance(t);
            PropertyDescriptorCollection col = TypeDescriptor.GetProperties(model);
            HashSet<string> list = new HashSet<string>();

            foreach (PropertyDescriptor item in col)
            {
                if (item.Description != null)
                {
                    if (da.Columns.Contains(item.Description))
                    {
                        list.Add(item.Name);
                    }
                }
            }

            foreach (DataRow dr in da.Rows)
            {
                model = Activator.CreateInstance(t);
                foreach (PropertyDescriptor item in col)
                {
                    if (list.Contains(item.Name) && (item.Description != null))
                    {
                        object value = ObjectExtension.GetValueByType(item.PropertyType, dr[item.Description]);

                        if (value != null)
                        {
                            item.SetValue(model, value);
                        }
                    }
                }
                if (model != null)
                {
                    result.Add((T)model);
                }
            }

            return result;
        }


        /// <summary>
        /// 查询类 name跟描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="outlist"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetDictionaryModelByDesc<T>(IList<string> outlist)
        {
            Type t = typeof(T);
            object Instance = Activator.CreateInstance(t);
            PropertyDescriptorCollection col = TypeDescriptor.GetProperties(Instance);
            IDictionary<string, string> result = new Dictionary<string, string>();

            foreach (PropertyDescriptor item in col)
            {
                if (item.Description != null)
                {
                    if (outlist != null && outlist.Count > 0)
                    {
                        if (!outlist.Contains(item.Name))
                        {
                            result.Add(item.Name.ToLower(), item.Description);
                        }
                    }
                    else
                    {
                        result.Add(item.Name.ToLower(), item.Description);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取model制定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValueByModelName<T>(T model, string name)
        {
            string result = String.Empty;
            PropertyDescriptorCollection col = TypeDescriptor.GetProperties(model);
            foreach (PropertyDescriptor item in col)
            {
                if (item.Name.ToLower() == name.ToLower())
                {
                    result = item.GetValue(model).ToT<string>();
                    break;
                }
            }
            return result;
        }

    }
}
