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
using System.Threading;
using System.ComponentModel;
using MonoTouch.Foundation;

namespace FieldService.iOS
{
	/// <summary>
	/// Synchronize invoke implementation for iOS
	/// </summary>
	public class SynchronizeInvoke : NSObject, ISynchronizeInvoke
	{
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

			//Uses NSObject.BeginInvokeOnMainThread
			BeginInvokeOnMainThread (() => {
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
			//Uses NSObject.InvokeOnMainThread
			object result = null;
			InvokeOnMainThread (() => result = method.DynamicInvoke (args));
			return result;
		}
		
		public bool InvokeRequired
		{
			get { return !NSThread.IsMain; }
		}
	}
}

