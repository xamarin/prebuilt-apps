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
        }

		protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo (e);

			var viewModel = new FavoritesViewModel (App.Current.FavoritesRepository, true);
			DataContext = viewModel;
			InstructionsText.Visibility = viewModel.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
		}

		void HandleSearchClicked (object sender, EventArgs e)
		{
			NavigationService.Navigate (new Uri ("/SearchPage.xaml", UriKind.Relative));
		}
    }
}
