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
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
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
		/// <summary>
		/// Occurs when dismissed.
		/// </summary>
		public event EventHandler Dismissed;

		readonly AssignmentViewModel assignmentViewModel;
		readonly LaborViewModel laborViewModel;
		UIBarButtonItem labor, space1, space2, done;
		TableSource tableSource;

		public AddLaborController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			laborViewModel = ServiceContainer.Resolve<LaborViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			cancel.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);
			
			var label = new UILabel (new RectangleF(0, 0, 80, 36)) { 
				Text = "Labor",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (18),
			};
			labor = new UIBarButtonItem(label);

			done = new UIBarButtonItem("Done", UIBarButtonItemStyle.Bordered, (sender, e) => {
				laborViewModel
					.SaveLaborAsync (assignmentViewModel.SelectedAssignment, laborViewModel.SelectedLabor)
					.ContinueWith (_ => BeginInvokeOnMainThread (() => DismissViewController (true, null)));
			});
			done.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);
			done.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			
			space1 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			space2 = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

			tableView.Source = 
				tableSource = new TableSource();
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
			if (method != null) {
				method(this, EventArgs.Empty);
			}
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

				typeCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				typeCell.TextLabel.Text = "Type";
				typeCell.AccessoryView = type = new LaborTypeTextField (new RectangleF(0, 0, 200, 36))
				{
					TextAlignment = UITextAlignment.Right,
					VerticalAlignment = UIControlContentVerticalAlignment.Center,
					BackgroundColor = UIColor.Clear,
				};
				typeCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				type.EditingDidEnd += (sender, e) => {
					laborViewModel.SelectedLabor.Type = type.LaborType;
				};

				hoursCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				hoursCell.TextLabel.Text = "Hours";
				hoursCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				hoursCell.AccessoryView = hours = new HoursField(new RectangleF(0, 0, 200, 44));
				hours.ValueChanged += (sender, e) => laborViewModel.SelectedLabor.Hours = TimeSpan.FromHours (hours.Value);

				descriptionCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				descriptionCell.AccessoryView = description = new PlaceholderTextView(new RectangleF(0, 0, 470, 400))
				{
					BackgroundColor = UIColor.Clear,
					TextColor = Theme.BlueTextColor,
					Placeholder = "Please enter notes here",
				};
				descriptionCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				description.SetDidChangeNotification (d => {
					if (description.Text != description.Placeholder) {
						laborViewModel.SelectedLabor.Description = d.Text;
					} else {
						laborViewModel.SelectedLabor.Description = string.Empty;
					}
				});
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

			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return indexPath.Section == 1 ? 410 : 44;
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return 2;
			}
			
			public override int RowsInSection (UITableView tableview, int section)
			{
				return section == 0 ? 2 : 1;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (enabled) {
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
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0) {
					return indexPath.Row == 0 ? typeCell : hoursCell;
				} else {
					return descriptionCell;
				}
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
