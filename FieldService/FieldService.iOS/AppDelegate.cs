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

using Foundation;
using UIKit;

using FieldService.Utilities;
using FieldService.ViewModels;

using Oracle.Cloud.Mobile;
using Oracle.Cloud.Mobile.MobileBackend;
using System.Reflection;
using Oracle.Cloud.Mobile.Configuration;
using Oracle.Cloud.Mobile.Notifications;
using System.Threading.Tasks;
using PushNotification.Plugin;

namespace FieldService.iOS
{
	/// <summary>
	/// AppDelegate, the main callback for application-level events in iOS
	/// </summary>
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIStoryboard storyboard;
		LoginController loginController;

		public override UIWindow Window { get; set; }

		/// <summary>
		/// This the main entry point for the app on iOS
		/// </summary>
		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			//Create our window
			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			//Register some services
			ServiceContainer.Register (Window);
			ServiceContainer.Register <ISynchronizeInvoke>(() => new SynchronizeInvoke());

			//Register for Oracle MCS services
			var json = ResourceLoader.GetEmbeddedResourceStream(Assembly.GetAssembly(typeof(Application)),
				"McsConfiguration.json");
			MobileBackendManager.Manager.Configuration = new MobileBackendManagerConfiguration(json);

			//You must authenticate first
			Task.Run(async () => {
				bool result = await MobileBackendManager.Manager.DefaultMobileBackend.Authorization.AuthenticateAsync("fieldmanager", "qR3ctrnk");
				if (!result)
					System.Diagnostics.Debug.WriteLine("Login failed.");
			}).Wait();

			//Consider inizializing before application initialization, if using any CrossPushNotification method during application initialization.
			CrossPushNotification.Initialize<FieldService.Shared.FieldServicePushNotificationListener> ();
			CrossPushNotification.Current.Register ();


			//Apply our UI theme
			Theme.Apply ();

			//Load our storyboard and setup our UIWindow and first view controller
			storyboard = UIStoryboard.FromName ("MainStoryboard", null);

			//loginController = storyboard.InstantiateInitialViewController () as LoginController;
			//Window.RootViewController = loginController;

			var tabController = storyboard.InstantiateViewController<TabController>();
			Window.RootViewController = tabController;

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
			if (loginViewModel.IsInactive)
				Theme.TransitionController (loginController, false);

			//Let's reset the time, just to be safe
			loginViewModel.ResetInactiveTime ();
		}


		//Push Notification Events
		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnErrorReceived(error);
			}
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnRegisteredSuccess(deviceToken);
			}
		}

		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications();
		}

		public override void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);
			}		
		}


		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{ 
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);
			}
		}
	}
}

