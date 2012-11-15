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

using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;

using EmployeeDirectory.ViewModels;
using EmployeeDirectory.Data;

namespace EmployeeDirectory.iOS
{
	public class PersonViewController : UITableViewController
	{
		PersonViewModel personViewModel;

		public PersonViewController (Person person, IFavoritesRepository favoritesRepository)
			: base (UITableViewStyle.Grouped)
		{
			personViewModel = new PersonViewModel (person, favoritesRepository);

			Title = person.SafeDisplayName;

			TableView.DataSource = new PersonDataSource (this);
			TableView.Delegate = new PersonDelegate (this);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			DeselectAll ();
		}

		void DeselectAll ()
		{
			var sel = TableView.IndexPathForSelectedRow;
			if (sel != null) {
				TableView.DeselectRow (sel, true);
			}
		}

		void OnPhoneSelected (string phoneNumber)
		{
			UIApplication.SharedApplication.OpenUrl (
				NSUrl.FromString ("tel:" + Uri.EscapeDataString (phoneNumber)));
		}

		void OnEmailSelected (string emailAddress)
		{
			if (MFMessageComposeViewController.CanSendText) {
				var composer = new MFMessageComposeViewController () {
					Recipients = new[] { emailAddress },
				};
				composer.Finished += (sender, e) => DismissViewController (true, null);
				PresentViewController (composer, true, null);
			}
		}

		void OnTwitterSelected (string twitterName)
		{
			var name = twitterName;
			if (name.StartsWith ("@")) {
				name = name.Substring (1);
			}
			UIApplication.SharedApplication.OpenUrl (
				NSUrl.FromString ("http://twitter.com/" + Uri.EscapeDataString (name)));
		}

		void OnUrlSelected (string url)
		{
			UIApplication.SharedApplication.OpenUrl (
				NSUrl.FromString (url));
		}

		void OnAddressSelected (string address)
		{
			UIApplication.SharedApplication.OpenUrl (
				NSUrl.FromString ("http://maps.google.com/maps?q=" + Uri.EscapeDataString (address)));
		}

		void OnPropertySelected (PersonViewModel.Property property)
		{
			switch (property.Type) {
			case PersonViewModel.PropertyType.Phone:
				OnPhoneSelected (property.Value);
				break;
			case PersonViewModel.PropertyType.Email:
				OnEmailSelected (property.Value);
				break;
			case PersonViewModel.PropertyType.Twitter:
				OnTwitterSelected (property.Value);
				break;
			case PersonViewModel.PropertyType.Url:
				OnUrlSelected (property.Value);
				break;
			case PersonViewModel.PropertyType.Address:
				OnAddressSelected (property.Value);
				break;
			}
			DeselectAll ();
		}

		class PersonDelegate : UITableViewDelegate
		{
			PersonViewController controller;
			public PersonDelegate (PersonViewController controller)
			{
				this.controller = controller;
			}
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var prop = controller.personViewModel.PropertyGroups[indexPath.Section].Properties[indexPath.Row];
				controller.OnPropertySelected (prop);
			}
		}

		class PersonDataSource : UITableViewDataSource
		{
			PersonViewController controller;
			public PersonDataSource (PersonViewController controller)
			{
				this.controller = controller;
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return controller.personViewModel.PropertyGroups.Count;
			}

			public override int RowsInSection (UITableView tableView, int section)
			{
				return controller.personViewModel.PropertyGroups[section].Properties.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell ("C");
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Value2, "C");
				}

				var prop = controller.personViewModel.PropertyGroups[indexPath.Section].Properties[indexPath.Row];

				cell.TextLabel.Text = prop.Name.ToLowerInvariant ();
				cell.DetailTextLabel.Text = prop.Value;

				cell.SelectionStyle = prop.Type == PersonViewModel.PropertyType.Generic ?
						UITableViewCellSelectionStyle.None :
						UITableViewCellSelectionStyle.Blue;

				return cell;
			}
		}
	}
}

