//
//  Copyright 2012  Xamarin Inc.
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
using System;
using System.Linq;

using CoreGraphics;
using Foundation;
using UIKit;

using FieldService.Data;
using FieldService.ViewModels;
using FieldService.Utilities;
using Oracle.Cloud.Mobile;
using Oracle.Cloud.Mobile.MobileBackend;
using System.Net.Http;
using Newtonsoft.Json;

namespace FieldService.iOS
{
	/// <summary>
	/// Modal view controller for adding items to an assignment
	/// </summary>
	public partial class AddItemController : UIViewController
	{
		readonly ItemViewModel itemViewModel;

		/// <summary>
		/// Occurs when dismissed.
		/// </summary>
		public event EventHandler Dismissed;

		public AddItemController (IntPtr handle) : base (handle)
		{
			itemViewModel = ServiceContainer.Resolve<ItemViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			cancel.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
			cancel.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			var label = new UILabel (new CGRect (0f, 0f, 80f, 36f)) {
				Text = "Items",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (18),
			};
			var items = new UIBarButtonItem (label);

			toolbar.Items = new UIBarButtonItem[] {
				cancel,
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				items,
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
			};

			var source = new TableSource (this);
			tableView.Source = source;
			var searchDataSource = new SearchSource (this);

			source.ManagerApprovalRequired += async (object sender, EventArgs e) => {
				var okCancelAlertController = UIAlertController.Create ("Approval Require", "This item requires manager approval", UIAlertControllerStyle.Alert);
				okCancelAlertController.AddAction (UIAlertAction.Create ("OK", UIAlertActionStyle.Default, alert => Console.WriteLine ("Okay was clicked")));
				PresentViewController (okCancelAlertController, true, null);

				//Send API call to Oracle MCS to request Manager Approval

				var approvalsURI = new Uri(MobileBackendManager.Manager.DefaultMobileBackend.CustomCodeUri + "/FieldServiceAPI/workorderapprovals");

				using (var client = MobileBackendManager.Manager.DefaultMobileBackend.CreateHttpClient())
				{
					Item item = (Item)sender;
					var jsonContent = String.Format("{{\"CustomerName\": \"{0}\",\"Status\": \"Open\",\"Description\": \"Item approval required\",\"RequestDate\": \"{1}\",\"Part\": \"{2}\",\"PartDescription\": \"Flux Capacitor\",\"Price\": 8888.88,\"StatusDescription\": null}}", "NEED COMPNAME", DateTime.Now.ToShortDateString(), item.Name);
					//string jsonContent = "";
					HttpContent content = new StringContent(jsonContent);


					var contentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
					content.Headers.ContentType = contentType;

					var httpResponseMsg = await client.PostAsync(approvalsURI, content).ConfigureAwait(false);
				}
			};

			SearchDisplayController.SearchResultsSource = searchDataSource;
			SearchDisplayController.Delegate = new SearchDisplay (tableView, searchDataSource);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Reload items
			itemViewModel.LoadItemsAsync ().ContinueWith (_ => BeginInvokeOnMainThread (tableView.ReloadData));
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			
			var method = Dismissed;
			if (method != null)
				method(this, EventArgs.Empty);
		}

		/// <summary>
		/// Event when cancel button is clicked
		/// </summary>
		partial void Cancel (NSObject sender)
		{
			DismissViewController (true, null);
		}

		/// <summary>
		/// Table source for the list of items
		/// </summary>
		private class TableSource : UITableViewSource
		{
			const string Identifier = "ItemCell";
			readonly AddItemController controller;
			readonly AssignmentViewModel assignmentViewModel;
			protected readonly ItemViewModel itemViewModel;
			public event EventHandler ManagerApprovalRequired;

			public TableSource (AddItemController controller)
			{
				this.controller = controller;
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
				itemViewModel = ServiceContainer.Resolve<ItemViewModel>();
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return itemViewModel.Items == null ? 0 : itemViewModel.Items.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var item = GetItem (indexPath);
				var cell = tableView.DequeueReusableCell (Identifier);
				if (cell == null)
					cell = new UITableViewCell(UITableViewCellStyle.Default, Identifier);

				cell.TextLabel.Text = string.Format ("{0} {1}", item.Name, item.Number);
				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var item = GetItem (indexPath);
				tableView.UserInteractionEnabled = false;

				//Check if an item that requires manager approval
				if (item.Id == 9) {
					EventHandler handler = ManagerApprovalRequired;
					if (handler != null)
						handler (item, new EventArgs ());
				}

				itemViewModel.SaveAssignmentItemAsync (assignmentViewModel.SelectedAssignment, new AssignmentItem {
					ItemId = item.Id,
					AssignmentId = assignmentViewModel.SelectedAssignment.Id,
				})
				
				.ContinueWith(_ => {
					BeginInvokeOnMainThread (() => {
						tableView.UserInteractionEnabled = true;
						if (item.Id != 9) {
							controller.DismissViewController (true, null);
						}
					});
				});
			}

			protected virtual Item GetItem(NSIndexPath indexPath)
			{
				return itemViewModel.Items[indexPath.Row];
			}
		}

		/// <summary>
		/// Search source for the list of items
		/// </summary>
		private class SearchSource : TableSource, ISearchSource
		{
			public string SearchText { get; set; }

			public SearchSource (AddItemController controller)
				: base(controller)
			{
				
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return itemViewModel.Items == null ? 0 : itemViewModel.Items.Count (Filter);
			}

			protected override Item GetItem (NSIndexPath indexPath)
			{
				return itemViewModel.Items.Where (Filter).Skip (indexPath.Row).First ();
			}

			private bool Filter(Item item)
			{
				return !string.IsNullOrEmpty (SearchText) && 
					(item.Name.ToLower ().Contains (SearchText) || item.Number.ToLower().Contains (SearchText));
			}
		}
	}
}
