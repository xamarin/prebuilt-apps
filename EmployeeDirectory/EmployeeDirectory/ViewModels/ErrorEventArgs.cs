using System;

namespace EmployeeDirectory.ViewModels
{
	public class ErrorEventArgs : EventArgs
	{
		public Exception Exception { get; set; }

		public ErrorEventArgs (Exception exception)
		{
			Exception = exception;
		}
	}
}

