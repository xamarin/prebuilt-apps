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
		
		public LoginController (IntPtr handle) : base (handle)
		{
			loginViewModel = ServiceContainer.Resolve<LoginViewModel> ();

			//Hook up ViewModel events
			loginViewModel.IsBusyChanged += (sender, e) => {
				if (IsViewLoaded) {
					indicator.Hidden = !loginViewModel.IsBusy;
					login.Hidden = loginViewModel.IsBusy;
				}
			};
			loginViewModel.IsValidChanged += (sender, e) => {
				if (IsViewLoaded)
					login.Enabled = loginViewModel.IsValid;
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Theme.SetupControllers (Storyboard);

			//Set up any properties on views that must be done from code

			View.BackgroundColor = Theme.LinenPattern;
			box.Image = Theme.LoginBox;
			logoBackground.Image = Theme.LoginInset;
			login.SetBackgroundImage (Theme.LoginButton, UIControlState.Normal);

			//Text Fields
			//I used LeftView as a quick way to add padding to a "plain" styled UITextField

			username.LeftView = new UIView (new RectangleF (0, 0, 10, 10));
			username.LeftViewMode = UITextFieldViewMode.Always;
			username.Background = Theme.LoginTextField;
			username.SetDidChangeNotification (text => loginViewModel.Username = text.Text);
			username.ShouldReturn = _ => {
				password.BecomeFirstResponder ();
				return false;
			};

			password.LeftView = new UIView (new RectangleF (0, 0, 10, 10));
			password.LeftViewMode = UITextFieldViewMode.Always;
			password.Background = Theme.LoginTextField;
			password.SetDidChangeNotification (text => loginViewModel.Password = text.Text);
			password.ShouldReturn = _ => {
				if (loginViewModel.IsValid) {
					Login ();
				}
				return false;
			};
		}

		partial void Login ()
		{
			password.ResignFirstResponder ();
			
			loginViewModel.LoginAsync ().ContinueOnUIThread (_ => Theme.TransitionController<AssignmentsController>());
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

