using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace FieldService.Tests.Mocks {
    /// <summary>
    /// Simple implementation of ISynchronizeInvoke for testing
    /// </summary>
    class MockSynchronizeInvoke : ISynchronizeInvoke {

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
            var result = new AsyncResult ();

            ThreadPool.QueueUserWorkItem (delegate {
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
            return method.DynamicInvoke (args);
        }

        public bool InvokeRequired
        {
            get { return false; }
        }
    }
}
