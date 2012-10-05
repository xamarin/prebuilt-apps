using System;
using System.Windows;
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace EmployeeDirectory.WinPhone
{
	public partial class PersonPage : PhoneApplicationPage
	{
		public PersonPage ()
		{
			InitializeComponent ();
		}

		protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo (e);

			var src = NavigationContext.QueryString ["src"];
			var id = NavigationContext.QueryString ["id"];

			Person person = null;
			if (src == "favorites") {
				person = App.Current.FavoritesRepository.FindById (id);
			}

			if (person != null) {
				DataContext = new PersonViewModel (person, App.Current.FavoritesRepository);
			}
		}

		void HandleAddressTapped (string address)
		{
			var task = new BingMapsTask () {
				SearchTerm = address,
			};
			task.Show ();
		}

		void HandlePhoneTapped (string phoneNumber)
		{
			var task = new PhoneCallTask () {
				PhoneNumber = phoneNumber,
			};
			task.Show ();
		}

		void HandleEmailTapped (string to)
		{
			var task = new EmailComposeTask () {
				To = to,
			};
			task.Show ();
		}

		void HandleUrlTapped (Uri url)
		{
			var task = new WebBrowserTask () {
				Uri = url,
			};
			task.Show ();
		}

		void HandleTwitterTapped (string username)
		{
			var task = new WebBrowserTask () {
				Uri = new Uri ("http://twitter.com/" + username.Substring (1), UriKind.Absolute),
			};
			task.Show ();
		}

		void HandlePropertyTapped (object sender, System.Windows.Input.GestureEventArgs e)
		{
			var prop = ((FrameworkElement)sender).DataContext as PersonViewModel.Property;
			if (prop == null) return;

			switch (prop.Type) {
				case PersonViewModel.PropertyType.Address:
					HandleAddressTapped (prop.Value);
					break;
				case PersonViewModel.PropertyType.Phone:
					HandlePhoneTapped (prop.Value);
					break;
				case PersonViewModel.PropertyType.Email:
					HandleEmailTapped (prop.Value);
					break;
				case PersonViewModel.PropertyType.Url:
					HandleUrlTapped (new Uri (prop.Value));
					break;
				case PersonViewModel.PropertyType.Twitter:
					HandleTwitterTapped (prop.Value);
					break;
			}
		}
	}
}
