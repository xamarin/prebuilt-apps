using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Utilities {
    /// <summary>
    /// Class containing helper extension methods
    /// </summary>
    public static class Extensions {

        /// <summary>
        /// Attaches a continuation on a task on the UI Thread
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static Task ContinueOnUIThread (this Task task, Action<Task> callback)
        {
#if NCRUNCH
            return task.ContinueWith (callback);
#else
            return task.ContinueWith (callback, TaskScheduler.FromCurrentSynchronizationContext ());
#endif
        }

        /// <summary>
        /// Attaches a continuation on a task on the UI Thread
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static Task<T> ContinueOnUIThread<T> (this Task<T> task, Func<Task<T>, T> callback)
        {
#if NCRUNCH
            return task.ContinueWith<T> (callback);
#else
            return task.ContinueWith<T> (callback, TaskScheduler.FromCurrentSynchronizationContext ());
#endif
        }
    }
}
