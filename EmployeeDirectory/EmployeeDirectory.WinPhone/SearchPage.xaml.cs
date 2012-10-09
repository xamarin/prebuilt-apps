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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using EmployeeDirectory.ViewModels;

namespace EmployeeDirectory.WinPhone
{
	public partial class SearchPage : PhoneApplicationPage
	{
		public SearchPage ()
		{
			InitializeComponent ();

			DataContext = new SearchViewModel (App.Current.DirectoryService, App.Current.SavedSearch);

			Loaded += HandleLoaded;
		}

		SearchViewModel ViewModel { get { return (SearchViewModel)DataContext; } }

		bool IsValidSearchText
		{
			get
			{
				return !string.IsNullOrWhiteSpace (SearchText.Text);
			}
		}
		
		void HandleLoaded (object sender, RoutedEventArgs e)
		{			
			SearchText.Focus ();
			SearchText.Select (SearchText.Text.Length, 0);
		}
		
		void HandleSearchTextKeyDown (object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter) {
				HandleSearch (sender, new RoutedEventArgs ());
				SearchResults.Focus ();
			}
		}

		void HandleSearch (object sender, RoutedEventArgs e)
		{
			if (IsValidSearchText) {
				ViewModel.Search ();
			}
		}
	}
}
