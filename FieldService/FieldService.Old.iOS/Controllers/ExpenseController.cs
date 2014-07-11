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
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.Utilities;
using FieldService.ViewModels;
using FieldService.Data;

namespace FieldService.iOS
{
	/// <summary>
	/// Controller for the expenses section
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		readonly AssignmentViewModel assignmentViewModel;
		readonly ExpenseViewModel expenseViewModel;
		UILabel title;
		UIBarButtonItem titleButton, edit, addItem, space;

		public ExpenseController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI to setup from code
			View.BackgroundColor = Theme.BackgroundColor;
			title = new UILabel (new RectangleF (0, 0, 160, 36)) { 
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16),
				Text = "Expenses",
			};
			titleButton = new UIBarButtonItem (title);
			toolbar.Items = new UIBarButtonItem[] { titleButton };

			var textAttributes = new UITextAttributes { TextColor = UIColor.White };
			edit = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Bordered, delegate {
				edit.Title = tableView.Editing ? "Edit" : "Done";
				tableView.SetEditing (!tableView.Editing, true);
			});
			edit.SetTitleTextAttributes (textAttributes, UIControlState.Normal);
			edit.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			space = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			
			addItem = new UIBarButtonItem ("Add Expense", UIBarButtonItemStyle.Bordered, OnAddExpense);
			addItem.SetTitleTextAttributes (textAttributes, UIControlState.Normal);
			addItem.SetBackgroundImage (Theme.BlueBarButtonItem, UIControlState.Normal, UIBarMetrics.Default);

			tableView.Source = new TableSource (this);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			ReloadExpenses ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			//Dismiss editing on the tableView
			if (tableView.Editing) {
				edit.Title = "Edit";
				tableView.SetEditing (false, true);
			}
		}

		/// <summary>
		/// Reload the expenses
		/// </summary>
		public void ReloadExpenses ()
		{
			if (IsViewLoaded) {
				UpdateToolbar ();

				expenseViewModel.LoadExpensesAsync (assignmentViewModel.SelectedAssignment)
					.ContinueWith (_ => {
						BeginInvokeOnMainThread (() => {
							UpdateToolbar ();
							tableView.ReloadData ();
						});
					});
			}
		}

		/// <summary>
		/// Refreshes toolbar items and updates text
		/// </summary>
		private void UpdateToolbar()
		{
			var assignment = assignmentViewModel.SelectedAssignment;
			if (assignment.Status == AssignmentStatus.Complete || assignment.IsHistory) {
				toolbar.Items = new UIBarButtonItem[] { titleButton };
			} else if (expenseViewModel.Expenses == null || expenseViewModel.Expenses.Count == 0) {
				toolbar.Items = new UIBarButtonItem[] {
					titleButton,
					space,
					addItem
				};
			} else {
				toolbar.Items = new UIBarButtonItem[] {
					titleButton,
					space,
					edit,
					addItem
				};
			}
			toolbar.SetBackgroundImage (assignment.IsHistory ? Theme.OrangeBar : Theme.BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);

			//Update text
			if (expenseViewModel.Expenses == null || expenseViewModel.Expenses.Count == 0) 
				title.Text = "Expenses";
			else
				title.Text = string.Format ("Expenses (${0:0.00})", expenseViewModel.Expenses.Sum (e => e.Cost));
		}

		private void OnAddExpense (object sender, EventArgs e)
		{
			expenseViewModel.SelectedExpense = new Expense {
				AssignmentId = assignmentViewModel.SelectedAssignment.Id,
			};
			
			var addExpenseController = Storyboard.InstantiateViewController<AddExpenseController>();
			addExpenseController.Dismissed += (a, b) => ReloadExpenses ();
			PresentViewController (addExpenseController, true, null);
		}

		/// <summary>
		/// Table source for expenses
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly ExpenseController controller;
			readonly ExpenseViewModel expenseViewModel;
			readonly AssignmentViewModel assignmentViewModel;
			const string Identifier = "ExpenseCell";

			public TableSource (ExpenseController controller)
			{
				this.controller = controller;
				expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel>();
				assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return expenseViewModel.Expenses == null ? 0 : expenseViewModel.Expenses.Count;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				expenseViewModel.SelectedExpense = expenseViewModel.Expenses[indexPath.Row];

				var addExpenseController = controller.Storyboard.InstantiateViewController<AddExpenseController>();
				addExpenseController.Dismissed += (sender, e) => controller.ReloadExpenses ();
				controller.PresentViewController (addExpenseController, true, controller.ReloadExpenses);

				//Deselect the cell, a bug in Apple's UITableView requires BeginInvoke
				BeginInvokeOnMainThread (() => {
					var cell = tableView.CellAt (indexPath);
					cell.SetSelected (false, true);
				});
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				return !assignmentViewModel.SelectedAssignment.IsReadonly;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				expenseViewModel
					.DeleteExpenseAsync (assignmentViewModel.SelectedAssignment, expenseViewModel.Expenses[indexPath.Row])
					.ContinueWith (_ => BeginInvokeOnMainThread (controller.ReloadExpenses));
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var expense = expenseViewModel.Expenses [indexPath.Row];
				var cell = tableView.DequeueReusableCell (Identifier) as ExpenseCell;
				cell.SetExpense (expense);
				return cell;
			}
		}
	}
}
