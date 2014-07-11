using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Utilities;
using FieldService.ViewModels;
using System.ComponentModel;

namespace FieldService.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIStoryboard storyboard;
		LoginController loginController;

		public override UIWindow Window {
			get;
			set;
		}

		/// <summary>
		/// This the main entry point for the app on iOS
		/// </summary>
		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			//Create our window
			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			Console.WriteLine ("Hola mundo");
			Console.WriteLine ("NSBundle.MainBundle.ResourcePath" + NSBundle.MainBundle.ResourcePath);

			//Register some services
			ServiceContainer.Register (Window);
			ServiceContainer.Register <ISynchronizeInvoke>(() => new SynchronizeInvoke());

			//Apply our UI theme
			Theme.Apply ();

			//Load our storyboard and setup our UIWindow and first view controller
			storyboard = UIStoryboard.FromName ("MainStoryboard", null);
			Window.RootViewController = 
				loginController = storyboard.InstantiateInitialViewController () as LoginController;
			Window.MakeKeyAndVisible ();

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

