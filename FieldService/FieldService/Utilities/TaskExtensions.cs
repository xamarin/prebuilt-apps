//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Utilities {
    /// <summary>
    /// Class containing helper extension methods
    /// </summary>
    public static class TaskExtensions {

        /// <summary>
        /// Attaches a continuation on a task on the UI Thread
        /// </summary>
        /// <param name="task"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static Task ContinueOnCurrentThread (this Task task, Action<Task> callback)
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
        public static Task<T> ContinueOnCurrentThread<T> (this Task<T> task, Func<Task<T>, T> callback)
        {
#if NCRUNCH
            return task.ContinueWith<T> (callback);
#else
            return task.ContinueWith<T> (callback, TaskScheduler.FromCurrentSynchronizationContext ());
#endif
        }

        /// <summary>
        /// A quick helper to be able to chain 2 tasks together
        /// </summary>
        public static Task ContinueWith (this Task task, Task continuation)
        {
            return task.ContinueWith (t => continuation).Unwrap ();
        }

        /// <summary>
        /// A quick helper to be able to chain 2 tasks together
        /// </summary>
        public static Task<T> ContinueWith<T> (this Task task, Task<T> continuation)
        {
            return task.ContinueWith (t => continuation).Unwrap ();
        }
    }
}
