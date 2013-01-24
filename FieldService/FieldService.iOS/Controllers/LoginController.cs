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
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.ViewModels;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for the login screen
	/// </summary>
	public partial class LoginController : BaseController
	{
		readonly LoginViewModel loginViewModel;
		TabController tabController;
		
		public LoginController (IntPtr handle) : base (handle)
		{
			loginViewModel = ServiceContainer.Resolve<LoginViewModel>();

			//Hook up ViewModel events
			loginViewModel.IsBusyChanged += OnIsBusyChanged;
			loginViewModel.IsValidChanged += OnIsValidChanged;
		}

		private void OnIsBusyChanged (object sender, EventArgs e)
		{
			if (IsViewLoaded) {
				indicator.Hidden = !loginViewModel.IsBusy;
				login.Hidden = loginViewModel.IsBusy;
			}
		}

		private void OnIsValidChanged (object sender, EventArgs e)
		{
			if (IsViewLoaded)
				login.Enabled = loginViewModel.IsValid;
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);

			//Do this because the ViewModel hangs around for the lifetime of the app
			loginViewModel.IsBusyChanged -= OnIsBusyChanged;
			loginViewModel.IsValidChanged -= OnIsValidChanged;
		}

		public override bool HandlesKeyboardNotifications {
			get {
				return true;
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Set up any properties on views that must be done from code
			View.BackgroundColor = Theme.LinenPattern;
			box.Image = Theme.LoginBox;
			login.SetBackgroundImage (Theme.LoginButton, UIControlState.Normal);
			companyName.TextColor = Theme.LabelColor;
			questionMark.SetBackgroundImage (Theme.QuestionMark, UIControlState.Normal);

			//Text Fields
			//I used LeftView as a quick way to add padding to a "plain" styled UITextField

			username.LeftView = new UIView (new RectangleF (0, 0, 10, 10));
			username.LeftViewMode = UITextFieldViewMode.Always;
			username.TextColor = Theme.LabelColor;
			username.Background = Theme.LoginTextField;
			username.SetDidChangeNotification (text => loginViewModel.Username = text.Text);
			username.ShouldReturn = _ => {
				password.BecomeFirstResponder ();
				return false;
			};

			password.LeftView = new UIView (new RectangleF (0, 0, 10, 10));
			password.LeftViewMode = UITextFieldViewMode.Always;
			password.TextColor = Theme.LabelColor;
			password.Background = Theme.LoginTextField;
			password.SetDidChangeNotification (text => loginViewModel.Password = text.Text);
			password.ShouldReturn = _ => {
				if (loginViewModel.IsValid) {
					Login ();
				}
				return false;
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Clear out username/password
			username.Text = string.Empty;
			password.Text = string.Empty;

			//Animate the login box
			container.Alpha = 0;
			UIView.Animate (.3, 0, UIViewAnimationOptions.CurveEaseInOut, () => container.Alpha = 1, null);
		}

		partial void Login ()
		{
			//Dismiss the keyboard
			username.ResignFirstResponder ();
			password.ResignFirstResponder ();
			
			loginViewModel
				.LoginAsync ()
				.ContinueWith (_ => 
					BeginInvokeOnMainThread (() => {
						if (tabController == null)
							tabController = Storyboard.InstantiateViewController<TabController>();

						Theme.TransitionController(tabController);
					}));
		}

		partial void Help ()
		{
			new UIAlertView("Need Help?", "Enter any username or password.", null, "Ok").Show ();
		}

		protected override void OnKeyboardChanged (bool visible, float height)
		{
			//We "center" the popup when the keyboard appears/disappears
			var frame = container.Frame;
			if (visible)
				frame.Y -= height / 2;
			else
				frame.Y += height / 2;
			container.Frame = frame;
		}
	}
}

