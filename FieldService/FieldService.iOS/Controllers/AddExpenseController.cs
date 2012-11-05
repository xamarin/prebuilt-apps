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
		readonly ExpenseController expenseController;
		readonly AssignmentDetailsController detailController;
		readonly ExpenseViewModel expenseViewModel;
		TableSource tableSource;

		public AddExpenseController (IntPtr handle) : base (handle)
		{
			ServiceContainer.Register (this);

			expenseController = ServiceContainer.Resolve<ExpenseController>();
			detailController = ServiceContainer.Resolve<AssignmentDetailsController>();
			expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI setup from code
			cancel.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);
			cancel.SetBackgroundImage (Theme.BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			
			var label = new UILabel (new RectangleF(0, 0, 80, 36)) { 
				Text = "Labor",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (18),
			};
			var labor = new UIBarButtonItem(label);

			var done = new UIBarButtonItem("Done", UIBarButtonItemStyle.Bordered, (sender, e) => {
				expenseViewModel
					.SaveExpense (detailController.Assignment, expenseController.Expense)
					.ContinueOnUIThread (_ => DismissViewController (true, delegate { }));
			});
			done.SetTitleTextAttributes (new UITextAttributes() { TextColor = UIColor.White }, UIControlState.Normal);
			done.SetBackgroundImage (Theme.BarButtonItem, UIControlState.Normal, UIBarMetrics.Default);
			
			toolbar.Items = new UIBarButtonItem[] {
				cancel,
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				labor,
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				done,
			};

			tableView.Source = 
				tableSource = new TableSource();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//Load labor hours for the table
			tableSource.Load (expenseController.Expense);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			//Reload the labor on the other screen when dismissed
			expenseController.ReloadExpenses ();
		}

		/// <summary>
		/// Event when cancel button is clicked
		/// </summary>
		partial void Cancel (NSObject sender)
		{
			DismissViewController (true, delegate {	});
		}

		/// <summary>
		/// The table source - has static cells
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly ExpenseController expenseController;
			readonly UITableViewCell typeCell, hoursCell, descriptionCell;
			readonly UILabel type;
			readonly UITextField cost;
			readonly UITextView description;
			ExpenseCategorySheet expenseSheet;
			
			public TableSource ()
			{
				expenseController = ServiceContainer.Resolve<ExpenseController>();

				typeCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				typeCell.TextLabel.Text = "Type";
				typeCell.AccessoryView = type = new UILabel (new RectangleF(0, 0, 200, 36))
				{
					TextAlignment = UITextAlignment.Right,
					BackgroundColor = UIColor.Clear,
				};
				typeCell.SelectionStyle = UITableViewCellSelectionStyle.None;

				hoursCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				hoursCell.TextLabel.Text = "Hours";
				hoursCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				hoursCell.AccessoryView = cost = new UITextField(new RectangleF(0, 0, 200, 36))
				{
					VerticalAlignment = UIControlContentVerticalAlignment.Center,
					TextAlignment = UITextAlignment.Right,
				};
				cost.SetDidChangeNotification (c => 
				{
					string text = c.Text.Replace ("$", string.Empty);
					decimal value;
					if (decimal.TryParse (text, out value)) {
						expenseController.Expense.Cost = value;
					} else {
						expenseController.Expense.Cost = 0;
					}
				});

				descriptionCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				descriptionCell.AccessoryView = description = new UITextView(new RectangleF(0, 0, 470, 400))
				{
					BackgroundColor = UIColor.Clear,
					TextColor = Theme.LabelColor,
				};
				descriptionCell.SelectionStyle = UITableViewCellSelectionStyle.None;
				description.SetDidChangeNotification (d => expenseController.Expense.Description = d.Text);
			}

			public void Load (Expense expense)
			{
				type.Text = expense.Category.ToString ();
				cost.Text = expense.Cost.ToString ("$0.00");
				description.Text = expense.Description;
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
				if (indexPath.Section == 0) {
					if (indexPath.Row == 0) {
						//Category changed
						expenseSheet = new ExpenseCategorySheet();
						expenseSheet.Dismissed += (sender, e) => {
							var expense = expenseController.Expense;
							if (expenseSheet.Category.HasValue && expense.Category != expenseSheet.Category) {
								expense.Category = expenseSheet.Category.Value;

								Load (expense);
							}

							expenseSheet.Dispose ();
							expenseSheet = null;
						};
						expenseSheet.ShowFrom (typeCell.Frame, tableView, true);
					} else {
						//Give hours "focus"
						cost.BecomeFirstResponder ();
					}
				} else {
					//Give description "focus"
					description.BecomeFirstResponder ();
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
				cost.Dispose ();
				
				base.Dispose (disposing);
			}
		}
	}
}
