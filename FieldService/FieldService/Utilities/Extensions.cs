using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Utilities {
    public static class Extensions {

        public static Task ContinueOnUIThread (this Task task, Action<Task> callback)
        {
#if NCRUNCH
            return task.ContinueWith (callback);
#else
            return task.ContinueWith (callback, TaskScheduler.FromCurrentSynchronizationContext ());
#endif
        }

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
