using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EmployeeDirectory.Data;

namespace EmployeeDirectory.WinPhone
{
    public partial class App : Application
    {
		/// <summary>
		/// Gets the App object for the current application.
		/// </summary>
		public new static App Current { get { return (App)Application.Current; } }

		/// <summary>
		/// Gets the <see cref="IFavoritesRepository" /> for the application.
		/// </summary>
		public IFavoritesRepository FavoritesRepository { get; private set; }

		/// <summary>
		/// Gets the <see cref="IDirectoryService" /> for the application.
		/// </summary>
		public IDirectoryService DirectoryService { get; private set; }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
			// Load the directory
			var dataUri = new Uri("EmployeeDirectory.WinPhone;component/Data/XamarinDirectory.csv", UriKind.Relative);
			var dataInfo = GetResourceStream (dataUri);
			using (var reader = new System.IO.StreamReader (dataInfo.Stream)) {
				DirectoryService = new MemoryDirectoryService (new CsvReader<Person> (reader).ReadAll ());
			}

			// Load the favorites
			FavoritesRepository = XmlFavoritesRepository.Open ("Favorites.xml");
			FavoritesRepository.InsertOrUpdate (new Person {
				Name = "Frank A. Krueger",
				Title = "Developer",
				Email = "fak@praeclarum.org",
				WebPage = "http://praeclarum.org",
				Twitter = "@praeclarum",
				MobileNumbers = new List<string> {
					"555-123-4567",
				},
				Department = "Addons"
			});

			FavoritesRepository.InsertOrUpdate (new Person {
				Name = "Jo Ann Buckner",
				Title = "Lead",
				Department = "Marketing"
			});

			FavoritesRepository.InsertOrUpdate (new Person {
				Name = "James Clancey",
				Title = "PM",
				Department = "Addons",
				Email = "clancey@xamarin.com",
			});

			FavoritesRepository.InsertOrUpdate (new Person {
				Name = "James Christen",
				Title = "Master",
				Department = "Electronics"
			});
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}