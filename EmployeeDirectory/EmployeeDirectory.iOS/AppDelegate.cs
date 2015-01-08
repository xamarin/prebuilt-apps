//
//  Copyright 2012, Xamarin Inc.
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
//
using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;

namespace EmployeeDirectory.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		IDirectoryService service;

		FavoritesViewController favoritesViewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			if (!UIDevice.CurrentDevice.CheckSystemVersion (7, 0))
				Theme.Apply ();

			//
			// Create the service
			//

			// Local CSV file
			service = MemoryDirectoryService.FromCsv ("Data/XamarinDirectory.csv");

			//
			// Load the favorites
			//
			var favoritesRepository = XmlFavoritesRepository.OpenIsolatedStorage ("Favorites.xml");

			if (favoritesRepository.GetAll ().Count () == 0) {
				favoritesRepository = XmlFavoritesRepository.OpenFile ("Data/XamarinFavorites.xml");
				favoritesRepository.IsolatedStorageName = "Favorites.xml";
			}

			//
			// Load the last search
			//
			Search search = null;
			try {
				search = Search.Open ("Search.xml");
			} catch (Exception) {
				search = new Search ("Search.xml");
			}

			//
			// Build the UI
			//
			favoritesViewController = new FavoritesViewController (favoritesRepository, service, search);

			window = new UIWindow (UIScreen.MainScreen.Bounds) {
				RootViewController = new UINavigationController (favoritesViewController)
			};
			window.MakeKeyAndVisible ();

			//
			// Show the login screen at startup
			//
			var login = new LoginViewController (service);
			favoritesViewController.PresentViewController (login, false, null);

			return true;
		}

		public override void WillEnterForeground (UIApplication application)
		{
			if (LoginViewModel.ShouldShowLogin (Settings.LastUseTime)) {
				var login = new LoginViewController (service);
				favoritesViewController.PresentViewController (login, false, null);
			}
		}

		public override void DidEnterBackground (UIApplication application)
		{
			Settings.LastUseTime = DateTime.UtcNow;
		}

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

