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
//    limitations under the License.using System;
using System;

using CoreGraphics;
using Foundation;
using UIKit;

using FieldService.Utilities;
using FieldService.Data;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for manually adding labor to an assignment
	/// </summary>
	public partial class AddLaborController : BaseController
	{
		readonly AssignmentViewModel assignmentViewModel;
		readonly LaborViewModel laborViewModel;
		UIBarButtonItem labor, space1, space2, done;
		TableSource tableSource;

		/// <summary>
		/// Occurs when dismissed.
		/// </summary>
		public event EventHandler Dismissed;

		public AddLaborController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			laborViewModel = ServiceContainer.Resolve<LaborViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			cancel.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
			
			var label = new UILabel (new CGRect (0f, 0f, 80f, 36f)) { 
				Text = "Labor",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (18f),
			};
			labor = new UIBarButtonItem(label);

			done = new UIBarButtonItem("Done", UIBarButtonItemStyle.Bordered, (sender, e) => {
				laborViewModel
					.SaveLaborAsync (assignmentViewModel.SelectedAssignment, laborViewModel.SelectedLabor)
					.ContinueWith (_ => BeginInvokeOnMainThread (() => DismissViewController (true, null)));
			});
			done.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);
			done.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			
			space1 = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);
			space2 = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);

			tableView.Source = 
				tableSource = new TableSource ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Load labor hours for the table
			bool enabled = !assignmentViewModel.SelectedAssignment.IsReadonly;
			if (enabled) {
				toolbar.Items = new UIBarButtonItem[] {
					cancel,
					space1,
					labor,
					space2,
					done,
				};
				toolbar.SetBackgroundImage (Theme.BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);
				cancel.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			} else {
				toolbar.Items = new UIBarButtonItem[] { cancel, space1, labor, space2 };
				toolbar.SetBackgroundImage (Theme.OrangeBar, UIToolbarPosition.Any, UIBarMetrics.Default);
				cancel.SetBackgroundImage (Theme.OrangeBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			}
			tableSource.Load (enabled);
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
		/// Overriding this allows us to dismiss the keyboard programmatically from a modal controller
		/// </summary>
		public override bool DisablesAutomaticKeyboardDismissal {
			get {
				return false;
			}
		}

		/// <summary>
		/// The table source - has static cells
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly LaborViewModel laborViewModel;
			readonly UITableViewCell typeCell, hoursCell, descriptionCell;
			readonly LaborTypeTextField type;
			readonly PlaceholderTextView description;
			readonly HoursField hours;
			bool enabled;
			
			public TableSource ()
			{
				laborViewModel = ServiceContainer.Resolve<LaborViewModel>();

				typeCell = new UITableViewCell (UITableViewCellStyle.Default, null) {
					SelectionStyle = UITableViewCellSelectionStyle.None
				};
				typeCell.TextLabel.Text = "Type";
				typeCell.AccessoryView = type = new LaborTypeTextField (new CGRect (0f, 0f, 200f, 36f)) {
					TextAlignment = UITextAlignment.Right,
					VerticalAlignment = UIControlContentVerticalAlignment.Center,
					BackgroundColor = UIColor.Clear,
				};

				type.EditingDidEnd += (sender, e) => {
					laborViewModel.SelectedLabor.Type = type.LaborType;
				};

				hoursCell = new UITableViewCell (UITableViewCellStyle.Default, null) {
					SelectionStyle = UITableViewCellSelectionStyle.None
				};
				hoursCell.TextLabel.Text = "Hours";
				hoursCell.AccessoryView = hours = new HoursField (new CGRect (0f, 0f, 200f, 44f));
				hours.ValueChanged += (sender, e) => laborViewModel.SelectedLabor.Hours = TimeSpan.FromHours (hours.Value);

				descriptionCell = new UITableViewCell (UITableViewCellStyle.Default, null) {
					SelectionStyle = UITableViewCellSelectionStyle.None
				};
				descriptionCell.AccessoryView = description = new PlaceholderTextView(new CGRect (0f, 0f, Theme.IsiOS7 ? 515f : 470f, 400f)) {
					BackgroundColor = UIColor.Clear,
					TextColor = Theme.BlueTextColor,
					Placeholder = "Please enter notes here",
				};
				description.SetDidChangeNotification (d =>
					laborViewModel.SelectedLabor.Description = description.Text != description.Placeholder ? d.Text : string.Empty
				);
			}

			public void Load (bool enabled)
			{
				this.enabled = enabled;
				var labor = laborViewModel.SelectedLabor;

				type.Enabled =
					hours.Enabled =
					description.UserInteractionEnabled = enabled;
				type.TextColor = 
					hours.TextColor =
					description.TextColor = enabled ? Theme.BlueTextColor : UIColor.LightGray;

				type.LaborType = labor.Type;
				hours.Value = labor.Hours.TotalHours;
				description.Text = string.IsNullOrEmpty (labor.Description) ? description.Placeholder : labor.Description;
			}

			public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return indexPath.Section == 1 ? 410f : 44f;
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return 2;
			}
			
			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return section == 0 ? 2 : 1;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (!enabled)
					return;
				
				if (indexPath.Section == 0) {
					if (indexPath.Row == 0) {
						//Give type "focus"
						type.BecomeFirstResponder ();
					} else {
						//Give hours "focus"
						hours.BecomeFirstResponder ();
					}
				} else {
					//Give description "focus"
					description.BecomeFirstResponder ();
				}
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0)
					return indexPath.Row == 0 ? typeCell : hoursCell;
				else
					return descriptionCell;
			}
			
			protected override void Dispose (bool disposing)
			{
				typeCell.Dispose ();
				hoursCell.Dispose ();
				descriptionCell.Dispose ();
				type.Dispose ();
				description.Dispose ();
				hours.Dispose ();
				
				base.Dispose (disposing);
			}
		}
	}
}
