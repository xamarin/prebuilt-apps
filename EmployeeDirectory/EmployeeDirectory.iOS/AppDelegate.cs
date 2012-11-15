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

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using EmployeeDirectory.Data;

namespace EmployeeDirectory.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//
			// Create the service
			//

			// Local CSV file
//			var service = MemoryDirectoryService.FromCsv ("Data/XamarinDirectory.csv");

			// LDAP service
			var service = new LdapDirectoryService {
				Host = "ldap.mit.edu",
				SearchBase = "dc=mit,dc=edu",
			};

			//
			// Load the favorites
			//
			var favoritesRepository = XmlFavoritesRepository.Open ("Favorites.xml");

			//
			// Load the last search
			//
			Search search = null;
			try {
				search = Search.Open ("Search.xml");
			}
			catch (Exception) {
				search = new Search ("Search.xml");
			}

			//
			// Build the UI
			//
			var favoritesViewController = new FavoritesViewController (favoritesRepository, service, search);

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (favoritesViewController);
			window.MakeKeyAndVisible ();			
			return true;
		}

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

