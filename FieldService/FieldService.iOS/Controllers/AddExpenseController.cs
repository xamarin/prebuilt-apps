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
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using FieldService.ViewModels;
using FieldService.Utilities;
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for adding an expense report to an assignment
	/// </summary>
	public partial class AddExpenseController : BaseController
	{
		/// <summary>
		/// Occurs when dismissed.
		/// </summary>
		public event EventHandler Dismissed;

		readonly AssignmentViewModel assignmentViewModel;
		readonly ExpenseViewModel expenseViewModel;
		UIBarButtonItem expense, space1, space2, done;
		TableSource tableSource;

		public AddExpenseController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			cancel.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);
			
			var label = new UILabel (new RectangleF(0, 0, 80, 36)) { 
				Text = "Expense",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (18),
			};
			expense = new UIBarButtonItem(label);

			done = new UIBarButtonItem("Done", UIBarButtonItemStyle.Bordered, (sender, e) => {
				//Save the expense
				var task = expenseViewModel.SaveExpenseAsync (assignmentViewModel.SelectedAssignment, expenseViewModel.SelectedExpense);
				//Save the photo if we need to
				if (expenseViewModel.Photo != null) {
					task = task
						.ContinueWith (_ => expenseViewModel.Photo.ExpenseId = expenseViewModel.SelectedExpense.Id)
						.ContinueWith (expenseViewModel.SavePhotoAsync ());
				}
				//Dismiss the controller after the other tasks
				task.ContinueWith (_ => BeginInvokeOnMainThread(() => DismissViewController (true, null)));
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
					expense,
					space2,
					done,
				};
				toolbar.SetBackgroundImage (Theme.BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);
				cancel.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			} else {
				toolbar.Items = new UIBarButtonItem[] { cancel, space1, expense, space2 };
				toolbar.SetBackgroundImage (Theme.OrangeBar, UIToolbarPosition.Any, UIBarMetrics.Default);
				cancel.SetBackgroundImage (Theme.OrangeBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			}

			if (expenseViewModel.SelectedExpense.Id == 0) {
				expenseViewModel.Photo = null;
				tableSource.Load (enabled);
			} else {
				expenseViewModel.LoadPhotoAsync (expenseViewModel.SelectedExpense)
					.ContinueWith (_ => BeginInvokeOnMainThread (() => tableSource.Load (enabled)));
			}
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
		/// The table source - has static cells
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly ExpenseViewModel expenseViewModel;
			readonly UITableViewCell categoryCell, costCell, descriptionCell, photoCell;
			readonly UILabel category;
			readonly UITextField cost;
			readonly PlaceholderTextView description;
			readonly UIButton photoButton;
			readonly UIImageView photo;
			ExpenseCategorySheet expenseSheet;
			PhotoAlertSheet photoSheet;
			bool enabled;
			
			public TableSource ()
			{
				expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel>();

				categoryCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				categoryCell.TextLabel.Text = "Category";
				categoryCell.AccessoryView = category = new UILabel (new RectangleF(0, 0, 200, 36))
				{
					TextAlignment = UITextAlignment.Right,
					BackgroundColor = UIColor.Clear,
				};
				categoryCell.SelectionStyle = UITableViewCellSelectionStyle.None;

				costCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				costCell.TextLabel.Text = "Cost";
				costCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				costCell.AccessoryView = cost = new UITextField(new RectangleF(0, 0, 200, 36))
				{
					VerticalAlignment = UIControlContentVerticalAlignment.Center,
					TextAlignment = UITextAlignment.Right,
				};
				cost.SetDidChangeNotification (c => 
				{
					string text = c.Text.Replace ("$", string.Empty);
					decimal value;
					if (decimal.TryParse (text, out value)) {
						expenseViewModel.SelectedExpense.Cost = Math.Abs (value);
					} else {
						expenseViewModel.SelectedExpense.Cost = 0;
					}
				});

				descriptionCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				descriptionCell.AccessoryView = description = new PlaceholderTextView(new RectangleF(0, 0, 470, 90))
				{
					BackgroundColor = UIColor.Clear,
					Placeholder = "Please enter notes here",
				};
				descriptionCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				description.SetDidChangeNotification (d => {
					if (description.Text != description.Placeholder) {
						expenseViewModel.SelectedExpense.Description = d.Text;
					} else {
						expenseViewModel.SelectedExpense.Description = string.Empty;
					}
				});

				photoCell = new UITableViewCell(UITableViewCellStyle.Default, null);
				photoCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				photoButton = UIButton.FromType (UIButtonType.Custom);
				photoButton.SetBackgroundImage (Theme.AddPhoto, UIControlState.Normal);
				photoButton.SetTitle ("Add Photo", UIControlState.Normal);
				photoButton.SetTitleColor (Theme.LabelColor, UIControlState.Normal);
				photoButton.ContentEdgeInsets = new UIEdgeInsets(0, 0, 2, 0);
				photoButton.Frame = new RectangleF(210, 130, 115, 40);
				photoButton.TouchUpInside += (sender, e) => {
					if (photoSheet == null) {
						photoSheet = new PhotoAlertSheet();

						//Set the desired size for the resulting image
						var size = photo.Frame.Size;
						var scale = UIScreen.MainScreen.Scale;
						size.Width *= scale;
						size.Height *= scale;
						photoSheet.DesiredSize = size;

						//Set the callback for when the image is selected
						photoSheet.Callback = image => { 
							if (expenseViewModel.Photo == null)
							expenseViewModel.Photo = new ExpensePhoto { ExpenseId = expenseViewModel.SelectedExpense.Id };
							expenseViewModel.Photo.Image = image.ToByteArray ();
							Load (enabled);
						};
					}
					photoSheet.ShowFrom (photoButton.Frame, photoCell, true);
				};
				photoCell.AddSubview (photoButton);
				var frame = photoCell.Frame;
				frame.X = 18;
				frame.Width -= 34;
				photo = new UIImageView(frame);
				photo.AutoresizingMask = UIViewAutoresizing.All;
				photo.ContentMode = UIViewContentMode.ScaleAspectFit;
				photo.Layer.BorderWidth = 1;
				photo.Layer.BorderColor = new CGColor(0xcf, 0xcf, 0xcf, 0x7f);
				photo.Layer.CornerRadius = 10;
				photo.Layer.MasksToBounds = true;
				photoCell.AddSubview (photo);
			}

			public void Load (bool enabled)
			{
				this.enabled = enabled;
				var expense = expenseViewModel.SelectedExpense;

				cost.Enabled =
					description.UserInteractionEnabled = enabled;
				category.TextColor =
					cost.TextColor =
					description.TextColor = enabled ? Theme.BlueTextColor : UIColor.LightGray;

				category.Text = expense.Category.ToString ();
				cost.Text = expense.Cost.ToString ("$0.00");
				if (enabled)
					description.Text = string.IsNullOrEmpty (expense.Description) ? description.Placeholder : expense.Description;
				else
					description.Text = expense.Description;

				if (photo.Image != null) {
					photo.Image.Dispose ();
					photo.Image = null;
				}
				if (expenseViewModel.Photo != null) {
					photo.Hidden = false;
					photoButton.Hidden = true;
					photo.Image = expenseViewModel.Photo.Image.ToUIImage ();
				} else {
					photo.Hidden = true;
					photoButton.Hidden = !enabled;
				}
			}

			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				switch (indexPath.Section) {
				case 1:
					return 100;
				case 2:
					return 290;
				default:
					return 44;
				}
			}

			public override int NumberOfSections (UITableView tableView)
			{
				return 3;
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
							//Category changed
							expenseSheet = new ExpenseCategorySheet ();
							expenseSheet.Dismissed += (sender, e) => {
								var expense = expenseViewModel.SelectedExpense;
								if (expenseSheet.Category.HasValue && expense.Category != expenseSheet.Category) {
									expense.Category = expenseSheet.Category.Value;

									Load (enabled);
								}

								expenseSheet.Dispose ();
								expenseSheet = null;
							};
							expenseSheet.ShowFrom (categoryCell.Frame, tableView, true);
						} else {
							//Give hours "focus"
							cost.BecomeFirstResponder ();
						}
					} else if (indexPath.Section == 1) {
						//Give description "focus"
						description.BecomeFirstResponder ();
					}
				}
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0) {
					return indexPath.Row == 0 ? categoryCell : costCell;
				} else if (indexPath.Section == 1) {
					return descriptionCell;
				} else {
					return photoCell;
				}
			}
			
			protected override void Dispose (bool disposing)
			{
				categoryCell.Dispose ();
				costCell.Dispose ();
				photoCell.Dispose ();
				descriptionCell.Dispose ();
				category.Dispose ();
				description.Dispose ();
				cost.Dispose ();
				photoButton.Dispose ();
				photo.Dispose ();
				
				base.Dispose (disposing);
			}
		}
	}
}
