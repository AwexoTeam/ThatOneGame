using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Structure
{
    public static class Utils
    {
        public static T[] GetAllTypes<T>()
        {
            List<T> rtn = new List<T>();

            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type);

            foreach (var t in types)
            {
                if (t.GetType() == typeof(T))
                    continue;

                var cmd = Activator.CreateInstance(t);
                rtn.Add((T)cmd);
            }

            return rtn.ToArray();
        }

    }
}
