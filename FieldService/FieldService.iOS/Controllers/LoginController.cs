using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.ViewModels;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.iOS
{
	public partial class LoginController : BaseController
	{
		readonly LoginViewModel loginViewModel = new LoginViewModel(new DefaultService());

		public LoginController (IntPtr handle) : base (handle)
		{
			//Hook up ViewModel events
			loginViewModel.IsBusyChanged += (sender, e) => UpdateLoginButton ();
			loginViewModel.IsValidChanged += (sender, e) => UpdateLoginButton ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Set up any properties on views that must be done from code

			background.Image = Theme.LinenPattern;
			box.Image = Theme.LoginBox;
			logoBackground.Image = Theme.LoginInset;
			login.SetBackgroundImage (Theme.LoginButton, UIControlState.Normal);

			//Text Fields
			//I used LeftView as a quick way to add padding to a "plain" styled UITextField

			username.LeftView = new UIView(new RectangleF(0, 0, 10, 10));
			username.LeftViewMode = UITextFieldViewMode.Always;
			username.Background = Theme.LoginTextField;
			username.SetDidChangeNotification (text => loginViewModel.Username = text.Text);
			username.ShouldReturn = _ => {
				password.BecomeFirstResponder ();
				return false;
			};

			password.LeftView = new UIView(new RectangleF(0, 0, 10, 10));
			password.LeftViewMode = UITextFieldViewMode.Always;
			password.Background = Theme.LoginTextField;
			password.SetDidChangeNotification (text => loginViewModel.Password = text.Text);
			password.ShouldReturn = _ => {
				if (loginViewModel.IsValid) {
					password.ResignFirstResponder ();
					Login ();
				}
				return false;
			};

			login.TouchUpInside += (sender, e) => Login ();
		}

		private void Login()
		{
			loginViewModel.LoginAsync ()
				.ContinueOnUIThread (_ => {
					
					//We'll do something else here later
					new UIAlertView("Success!", "Logged in.", null, "OK").Show ();
				});
		}

		private void UpdateLoginButton()
		{
			if (IsViewLoaded)
				login.Enabled = !loginViewModel.IsBusy && loginViewModel.IsValid;
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			ReleaseDesignerOutlets ();
		}

		protected override void OnKeyboardChanged (bool visible, float height)
		{
			var frame = container.Frame;
			if (visible)
				frame.Y -= height / 2;
			else
				frame.Y += height / 2;
			container.Frame = frame;
		}
	}
}

