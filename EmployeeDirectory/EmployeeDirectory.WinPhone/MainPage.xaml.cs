using System;
using System.Windows;
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;
using Microsoft.Phone.Controls;

namespace EmployeeDirectory.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

			DataContext = new FavoritesViewModel (App.Current.FavoritesRepository, true);
        }

		void HandlePersonTapped (object sender, System.Windows.Input.GestureEventArgs e)
		{
			var person = ((FrameworkElement)sender).DataContext as Person;
			if (person == null) return;

			var url = string.Format (
				"/PersonPage.xaml?src=favorites&id={0}",
				Uri.EscapeDataString (person.Id));

			NavigationService.Navigate (new Uri (url, UriKind.Relative));
		}
    }
}
