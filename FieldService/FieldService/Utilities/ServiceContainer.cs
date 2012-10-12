using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Utilities {
    public static class ServiceContainer {
        static readonly Dictionary<Type, Lazy<object>> services = new Dictionary<Type, Lazy<object>> ();

        public static void Register<T> (T service)
        {
            services [typeof (T)] = new Lazy<object> (() => service);
        }

        public static void Register<T> ()
            where T : new ()
        {
            services [typeof (T)] = new Lazy<object> (() => Activator.CreateInstance (typeof (T)));
        }

        public static void Register<T> (Func<object> function)
        {
            services [typeof (T)] = new Lazy<object> (function);
        }

        public static T Resolve<T> ()
        {
            Lazy<object> service;
            if (services.TryGetValue (typeof (T), out service)) {
                return (T)service.Value;
            } else {
                throw new KeyNotFoundException (string.Format ("Service not found for type '{0}'", typeof (T)));
            }
        }
    }
}
