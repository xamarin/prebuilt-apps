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
using CoreGraphics;
using Foundation;
using UIKit;
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
				View.UserInteractionEnabled =
					indicator.Hidden = !loginViewModel.IsBusy;
				login.Hidden = loginViewModel.IsBusy;

				if (Theme.IsiOS7) {
					username.Hidden =
						password.Hidden = loginViewModel.IsBusy;
				}
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
			companyName.TextColor = Theme.LabelColor;
			questionMark.SetImage (Theme.QuestionMark, UIControlState.Normal);
			logo.Image = Theme.Logo;
			login.SetBackgroundImage (Theme.LoginButton, UIControlState.Normal);

			//Text Fields
			//I used LeftView as a quick way to add padding to a "plain" styled UITextField

			username.LeftViewMode = UITextFieldViewMode.Always;
			username.TextColor = Theme.LabelColor;
			username.SetDidChangeNotification (text => loginViewModel.Username = text.Text);
			username.ShouldReturn = _ => {
				password.BecomeFirstResponder ();
				return false;
			};

			password.LeftViewMode = UITextFieldViewMode.Always;
			password.TextColor = Theme.LabelColor;
			password.SetDidChangeNotification (text => loginViewModel.Password = text.Text);
			password.ShouldReturn = _ => {
				if (loginViewModel.IsValid) {
					Login ();
				}
				return false;
			};

			if (Theme.IsiOS7) {
				companyName.Hidden = true;
				username.LeftView = new UIView (new CGRect (0, 0, 20, 20));
				password.LeftView = new UIView (new CGRect (0, 0, 20, 20));
				login.Font = 
					username.Font =
					password.Font = Theme.FontOfSize (18);
				hexagons.Image = UIImage.FromFile ("Images/iOS7/hexagons.png");

				//Add light gray lines
				var view = new UIView (new CGRect (0, 0, container.Frame.Width, 1)) {
					BackgroundColor = Theme.LightGrayColor,
				};
				username.AddSubview (view);

				view = new UIView (new CGRect (0, 0, container.Frame.Width, 1)) {
					BackgroundColor = Theme.LightGrayColor,
				};
				password.AddSubview (view);

				//Resize the container
				var frame = container.Frame;
				frame.Width = 400;
				frame.Height = 300;
				frame.X = (View.Frame.Width - frame.Width) / 2;
				frame.Y = (View.Frame.Height - frame.Height) / 2;
				container.Frame = frame;

				//Move up the logo
				frame = logo.Frame;
				frame.Y -= 10;
				frame.Width = 172;
				frame.Height = 48;
				logo.Frame = frame;

				//Move the login button
				frame = login.Frame;
				frame.X = 0;
				frame.Y = container.Frame.Height - frame.Height;
				frame.Width = container.Frame.Width;
				login.Frame = frame;

				//Move the indicator
				frame = indicator.Frame;
				frame.Y = (container.Frame.Height - frame.Height) / 2;
				indicator.Frame = frame;

				//Move the username and password
				frame = username.Frame;
				frame.X = 0;
				frame.Y -= 60;
				frame.Width = container.Frame.Width;
				username.Frame = frame;

				frame = password.Frame;
				frame.X = 0;
				frame.Y -= 80;
				frame.Width = container.Frame.Width;
				password.Frame = frame;


			} else {
				username.Background =
					password.Background = Theme.LoginTextField;
				username.LeftView = new UIView (new CGRect (0, 0, 10, 10));
				password.LeftView = new UIView (new CGRect (0, 0, 10, 10));
			}
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

		protected override void OnKeyboardChanged (bool visible, nfloat height)
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

