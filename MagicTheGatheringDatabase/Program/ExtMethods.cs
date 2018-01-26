using System;
using System.Collections.Generic;
using System.Text;

namespace MagicDbContext
{
    public static class ExtMethods
    {
        public static T As<T>(this object val)
        {
            if (typeof(T).IsAssignableFrom(val.GetType())) return (T)val;
            return (T)Convert.ChangeType(val, typeof(T));
        }

        public static T AsOrDefault<T>(this object val)
        {
            try
            {
                return val.As<T>();
            }
            catch { }
            return default(T);
        }

        public static T GetValue<T>(this Dictionary<string, object> collection, string name)
        {
            if (collection.TryGetValue(name, out object value))
            {
                return value.As<T>();
            }
            throw new KeyNotFoundException();
        }

        public static T GetValueOrDefault<T>(this Dictionary<string, object> collection, string name)
        {
            try
            {
                return collection.GetValue<T>(name);
            }
            catch { }
            return default(T);
        }
    }
}
