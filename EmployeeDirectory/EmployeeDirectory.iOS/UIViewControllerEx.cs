using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace EmployeeDirectory.iOS
{
	/// <summary>
	/// UIViewController Extensions
	/// </summary>
	public static class UIViewControllerEx
	{
		public static void ShowError (this UIViewController vc, string title, Exception ex)
		{
			while (ex.InnerException != null) {
				ex = ex.InnerException;
			}
			ShowError (vc, title, ex.Message);
		}

		public static void ShowError (this UIViewController vc, string title, string message)
		{
			var alert = new UIAlertView (
				title,
				message,
				null,
				NSBundle.MainBundle.LocalizedString ("OK", "Error alert dimissal button title"));
			alert.Show ();
		}
	}
}

