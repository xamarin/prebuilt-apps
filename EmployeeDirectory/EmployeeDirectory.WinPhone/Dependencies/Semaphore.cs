using System;

namespace System.Threading
{
	/// <summary>
	/// This is a partial implementation of the Semaphore class from .NET
	/// to make up for Silverlight's missing class.
	/// </summary>
	public class Semaphore
	{
		readonly int maximumCount;
		int freeCount;

		readonly object mutex = new object ();
		readonly AutoResetEvent releaseEvent = new AutoResetEvent (false);

		public Semaphore (int initialCount, int maximumCount)
		{
			this.maximumCount = maximumCount;
			this.freeCount = initialCount;
		}

		/// <summary>
		/// Waits until the freeCount is greater than one
		/// </summary>
		public void WaitOne ()
		{
			while (true) {

				var gotOne = false;

				lock (mutex) {
					if (freeCount > 0) {
						gotOne = true;
						freeCount--;
					}
				}

				if (gotOne) {
					return;
				}
				else {
					releaseEvent.WaitOne ();
				}
			}
		}

		/// <summary>
		/// Returns a free slot
		/// </summary>
		public void Release ()
		{
			lock (mutex) {
				freeCount++;
			}
			releaseEvent.Set ();
		}
	}
}
