using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.ViewModels;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.iOS
{
	public partial class LoginController : UIViewController
	{
		readonly LoginViewModel loginViewModel = new LoginViewModel(new DefaultService());

		public LoginController (IntPtr handle) : base (handle)
		{
			loginViewModel.IsBusyChanged += (sender, e) => {
				if (login != null)
					login.Enabled = !loginViewModel.IsBusy && loginViewModel.IsValid;
			};
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		#region View lifecycle
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			username.EditingDidEnd += (sender, e) => loginViewModel.Username = username.Text;
			password.EditingDidEnd += (sender, e) => loginViewModel.Password = password.Text;

			//login.Enabled = false;
			login.TouchUpInside += (sender, e) => {
				var task = loginViewModel.LoginAsync ()
					.ContinueOnUIThread (_ => {
						new UIAlertView("Success!", "Logged in.", null, "OK").Show ();
					});
			};
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		#endregion
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return true;
		}
	}
}

