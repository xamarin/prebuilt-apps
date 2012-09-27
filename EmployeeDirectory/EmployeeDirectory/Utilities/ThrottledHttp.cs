using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace EmployeeDirectory.Utilities
{
	/// <summary>
	/// This class only allows a specific number of WebRequests to execute
	/// simultaneously.
	/// </summary>
	public class ThrottledHttp
	{
		Semaphore throttle;

		public TimeSpan Timeout { get; set; }

		public ThrottledHttp (int maxConcurrent)
		{
			throttle = new Semaphore (maxConcurrent, maxConcurrent);
			Timeout = TimeSpan.FromSeconds (5);
		}

		/// <summary>
		/// Get the specified resource. Blocks the thread until
		/// the throttling logic allows it to execute.
		/// </summary>
		/// <param name='uri'>
		/// The URI of the resource to get.
		/// </param>
		public Stream Get (Uri uri)
		{
			throttle.WaitOne ();

			var req = WebRequest.Create (uri);
			req.Timeout = (int)Timeout.TotalMilliseconds;

			var getTask = Task.Factory.FromAsync<WebResponse> (
				req.BeginGetResponse, req.EndGetResponse, null);

			return getTask.ContinueWith (task => {
				throttle.Release ();
				var res = task.Result;
				return res.GetResponseStream ();
			}).Result;
		}
	}
}

