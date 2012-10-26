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
using System.ComponentModel;
using System.Threading;
using Android.App;
using Android.OS;

namespace FieldService.Android.Utilities {
    /// <summary>
    /// Android implementation of ISynchronizeInvoke
    /// </summary>
    public class SynchronizeInvoke : ISynchronizeInvoke {

        /// <summary>
        /// An activity is required for RunOnUiThread
        /// </summary>
        public Activity Activity
        {
            get;
            set;
        }

        /// <summary>
        /// IAsyncResult implementation
        /// </summary>
        class AsyncResult : IAsyncResult {
            public object AsyncState
            {
                get;
                set;
            }

            public WaitHandle AsyncWaitHandle
            {
                get;
                set;
            }

            public bool CompletedSynchronously
            {
                get { return IsCompleted; }
            }

            public bool IsCompleted
            {
                get;
                set;
            }
        }

        public IAsyncResult BeginInvoke (Delegate method, object [] args)
        {
            if (Activity == null)
                throw new InvalidOperationException ("Activity is null!");

            var result = new AsyncResult ();

            Activity.RunOnUiThread (() => {
                result.AsyncWaitHandle = new ManualResetEvent (false);
                result.AsyncState = method.DynamicInvoke (args);
                result.IsCompleted = true;
            });

            return result;
        }

        public object EndInvoke (IAsyncResult result)
        {
            if (!result.IsCompleted) {
                result.AsyncWaitHandle.WaitOne ();
            }

            return result.AsyncState;
        }

        public object Invoke (Delegate method, object [] args)
        {
            if (Activity == null)
                throw new InvalidOperationException ("Activity is null!");

            object result = null;
            Activity.RunOnUiThread (() => {
                result = method.DynamicInvoke (args);
            });
            return result;
        }

        public bool InvokeRequired
        {
            get { return Looper.MyLooper () == Looper.MainLooper; }
        }
    }
}