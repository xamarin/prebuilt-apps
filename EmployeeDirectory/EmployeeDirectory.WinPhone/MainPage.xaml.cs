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
using System.Windows;
using EmployeeDirectory.Data;
using EmployeeDirectory.ViewModels;
using Microsoft.Phone.Controls;

namespace EmployeeDirectory.WinPhone {
    public partial class MainPage : PhoneApplicationPage {
        public MainPage ()
        {
            InitializeComponent ();
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
