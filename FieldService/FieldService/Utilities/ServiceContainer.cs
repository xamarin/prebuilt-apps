using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Utilities {
    public class ServiceContainer {
        static object locker = new object ();
        static ServiceContainer instance;

        private ServiceContainer ()
        {
            Services = new Dictionary<Type, Lazy<object>> ();
        }

        private Dictionary<Type, Lazy<object>> Services { get; set; }

        private static ServiceContainer Instance
        {
            get
            {
                lock (locker) {
                    if (instance == null)
                        instance = new ServiceContainer ();
                    return instance;
                }
            }
        }

        public static void Register<T> (T service)
        {
            Instance.Services [typeof (T)] = new Lazy<object> (() => service);
        }

        public static void Register<T> ()
            where T : new ()
        {
            Instance.Services [typeof (T)] = new Lazy<object> (() => new T ());
        }

        public static void Register<T> (Func<object> function)
        {
            Instance.Services [typeof (T)] = new Lazy<object> (function);
        }

        public static T Resolve<T> ()
        {
            Lazy<object> service;
            if (Instance.Services.TryGetValue (typeof (T), out service)) {
                return (T)service.Value;
            } else {
                throw new KeyNotFoundException (string.Format ("Service not found for type '{0}'", typeof (T)));
            }
        }
    }
}