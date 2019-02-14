using System;
using System.Collections.Generic;

namespace Jalan.Utils.Common
{
    /// <summary>
    /// 帮助获取特性的值
    /// 默认为自定义特性（必须重写ToString()）
    /// </summary>
    public class AttributeValueUtil
    {
        public static string[] GetAttributeValues<T>(Type temp, bool hasEmpty = true) where T : Attribute
        {
            var props = temp.GetProperties();
            List<string> result = new List<string>();
            if (hasEmpty)
                result.Add(string.Empty);
            foreach (var prop in props)
            {
                var array = prop.GetCustomAttributes(typeof(T), false);
                if (array.Length == 0)
                    continue;
                result.Add(array[0].ToString());
            }
            return result.ToArray();
        }
        /// <summary>
        /// 获取特性值与属性值的健值对
        /// </summary>
        /// <typeparam name="T">特性类</typeparam>
        /// <typeparam name="R">返回健值对中值的类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static Dictionary<string, TResult> GetAttributeValueAndPropertyValueDict<T, TResult>(object obj)
        {
            var type = obj.GetType();
            var props = type.GetProperties();
            Dictionary<string, TResult> result = new Dictionary<string, TResult>();
            foreach (var prop in props)
            {
                var array = prop.GetCustomAttributes(typeof(T), false);
                if (array.Length == 0)
                    continue;
                object o = prop.GetValue(obj, null);
                result.Add(array[0].ToString(), (TResult)o);
            }
            return result;
        }
        /// <summary>
        /// 获取属性与特性值的健值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPropertyAttributeValueDict<T>(Type type)
        {
            var props = type.GetProperties();
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var prop in props)
            {
                var array = prop.GetCustomAttributes(typeof(T), false);
                if (array.Length == 0)
                    continue;
                result.Add(prop.Name, array[0].ToString());
            }
            return result;
        }
        /// <summary>
        /// 获取枚举值与描述的键值对
        /// </summary>
        /// <typeparam name="T">特性类</typeparam>
        /// <typeparam name="TEnum">枚举类</typeparam>
        /// <param name="hasEmpty">是否需要空值（默认有空值）</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumValueAndDescriptionExtDict<T, TEnum>() where TEnum : struct
        {
            var type = typeof(TEnum);
            var props = type.GetFields();
            Dictionary<int, string> result = new Dictionary<int, string>();
            TEnum value;
            foreach (var prop in props)
            {
                var array = prop.GetCustomAttributes(typeof(T), false);
                if (array.Length == 0)
                    continue;
                Enum.TryParse<TEnum>(prop.Name, out value);
                var valueToInt = (int)(type.GetField(prop.Name).GetValue(value));
                result.Add(valueToInt, array[0].ToString());
            }
            return result;
        }
        /// <summary>
        /// 获取枚举的描述值集合
        /// </summary>
        /// <typeparam name="T">特性类</typeparam>
        /// <typeparam name="TEnum">枚举类</typeparam>
        /// <param name="hasEmpty">是否需要空值（默认有空值）</param>
        /// <returns></returns>
        public static List<string> GetEnumDescriptionExtNames<T, TEnum>(bool hasEmpty = true) where TEnum : struct
        {
            var type = typeof(TEnum);
            var props = type.GetFields();
            List<string> result = new List<string>();
            if (hasEmpty)
                result.Add(string.Empty);
            foreach (var prop in props)
            {
                var array = prop.GetCustomAttributes(typeof(T), false);
                if (array.Length == 0)
                    continue;
                result.Add(array[0].ToString());
            }
            return result;
        }
    }
}
