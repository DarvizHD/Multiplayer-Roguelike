using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Shared.Primitives;

namespace Backend.Extensions
{
    public static class SerializationExtensions
    {
        public static string GetString(this Dictionary<string, object> dictionary, string key)
        {
            return (string)dictionary[key];
        }

        public static int GetInt(this Dictionary<string, object> dictionary, string key)
        {
            return Convert.ToInt32(dictionary[key]);
        }

        public static float GetFloat(this Dictionary<string, object> dictionary, string key)
        {
            return Convert.ToSingle(dictionary[key]);
        }

        public static long GetLong(this Dictionary<string, object> dictionary, string key)
        {
            return Convert.ToInt64(dictionary[key]);
        }

        public static bool GetBool(this Dictionary<string, object> dictionary, string key)
        {
            return Convert.ToBoolean(dictionary[key]);
        }

        public static List<T> GetList<T>(this Dictionary<string, object> dictionary, string key)
        {
            return dictionary.GetList(key).Select(obj => (T)Convert.ChangeType(obj, typeof(T))).ToList();
        }

        public static Queue<T> GetQueue<T>(this Dictionary<string, object> dictionary, string key)
        {
            var list = dictionary.GetList(key);
            return new Queue<T>(list.Select(obj => (T)Convert.ChangeType(obj, typeof(T))));
        }

        public static T GetEnum<T>(this Dictionary<string, object> dictionary, string key) where T : Enum
        {
            var value = (string)dictionary[key];
            var type = typeof(T);

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
                if (attribute?.Value == value)
                {
                    return (T)field.GetValue(null);
                }
            }

            return (T)Enum.Parse(type, ToPascalCase(value), true);
        }

        public static T GetFlags<T>(this Dictionary<string, object> dictionary, string key) where T : Enum
        {
            var type = typeof(T);
            long combinedValue = 0;

            foreach (var item in (IEnumerable<object>)dictionary[key])
            {
                var str = item.ToString();

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
                    if (attribute?.Value == str)
                    {
                        combinedValue |= Convert.ToInt64(field.GetValue(null));
                        break;
                    }
                }
            }

            return (T)Enum.ToObject(type, combinedValue);
        }

        public static Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(this Dictionary<string, object> dictionary,
            string key)
        {
            var result = new Dictionary<TKey, TValue>();

            foreach (var pair in dictionary.GetNode(key))
            {
                var typedKey = (TKey)Convert.ChangeType(pair.Key, typeof(TKey));
                var value = (TValue)Convert.ChangeType(pair.Value, typeof(TValue));

                result[typedKey] = value;
            }

            return result;
        }

        public static Dictionary<string, object> GetNode(this Dictionary<string, object> dictionary, string key)
        {
            return (Dictionary<string, object>)dictionary[key];
        }

        public static Vector3 GetVector3(this Dictionary<string, object> dictionary, string key)
        {
            var list = (List<object>)dictionary[key];
            return new Vector3(
                Convert.ToInt32(list[0]),
                Convert.ToInt32(list[1]),
                Convert.ToInt32(list[2])
            );
        }

        public static Dictionary<string, object> ToJson<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var result = new Dictionary<string, object>();
            foreach (var kv in dictionary)
            {
                result[kv.Key.ToString()] = kv.Value;
            }

            return result;
        }

        public static void Set(this Dictionary<string, object> dictionary, string key, object value)
        {
            dictionary[key] = value;
        }

        public static List<object> ToList(this Vector3 vector)
        {
            return new List<object> { vector.X, vector.Y, vector.Z };
        }

        public static List<object> GetList(this Dictionary<string, object> dictionary, string key)
        {
            return (List<object>)dictionary[key];
        }

        private static string ToPascalCase(string value)
        {
            return string.Concat(value.Split('_', '-').Select(s => char.ToUpperInvariant(s[0]) + s[1..]));
        }
    }
}
