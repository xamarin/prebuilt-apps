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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// AppDelegate, the main callback for application-level events in iOS
	/// </summary>
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		UIStoryboard storyboard;
		LoginController loginController;

		/// <summary>
		/// This the main entry point for the app on iOS
		/// </summary>
		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			//Create our window
			window = new UIWindow(UIScreen.MainScreen.Bounds);

			//Register some services
			ServiceContainer.Register (window);
			ServiceContainer.Register <ISynchronizeInvoke>(() => new SynchronizeInvoke());

			//Apply our UI theme
			Theme.Apply ();

			//Load our storyboard and setup our UIWindow and first view controller
			storyboard = UIStoryboard.FromName ("MainStoryboard", null);
			window.RootViewController = 
				loginController = storyboard.InstantiateInitialViewController () as LoginController;
			window.MakeKeyAndVisible ();

			return true;
		}

		/// <summary>
		/// This is how orientation is setup on iOS 6
		/// </summary>
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations (UIApplication application, UIWindow forWindow)
		{
			return UIInterfaceOrientationMask.All;
		}

		/// <summary>
		/// Event when the app is backgrounded, or screen turned off
		/// </summary>
		public override void DidEnterBackground (UIApplication application)
		{
			var loginViewModel = ServiceContainer.Resolve<LoginViewModel>();

			loginViewModel.ResetInactiveTime ();
		}

		/// <summary>
		/// Event when the app is brought back to the foreground or screen unlocked
		/// </summary>
		public override void WillEnterForeground (UIApplication application)
		{
			var loginViewModel = ServiceContainer.Resolve<LoginViewModel>();
			if (loginViewModel.IsInactive) {
				Theme.TransitionController (loginController, false);
			}

			//Let's reset the time, just to be safe
			loginViewModel.ResetInactiveTime ();
		}
	}
}

