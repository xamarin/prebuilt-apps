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
			//
			//
			Search search = null;
			try {
				search = Search.OpenAsync ("Search").Result;
			}
			catch (Exception ex) {
				Console.WriteLine (ex);
				search = new Search ("Search");
			}

			//
			// Build the UI
			//
			var searchViewController = new SearchViewController (service, search);
			var favoritesViewController = new FavoritesViewController ();

			var tabs = new UITabBarController ();
			tabs.SetViewControllers (new UIViewController[] {
				new UINavigationController (searchViewController),
				new UINavigationController (favoritesViewController),
			}, false);

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = tabs;
			window.MakeKeyAndVisible ();			
			return true;
		}
	}
}

