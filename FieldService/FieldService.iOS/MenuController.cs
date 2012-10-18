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
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// This controller holds the main menu for assignments
	/// </summary>
	public partial class MenuController : UIViewController
	{
		readonly AssignmentViewModel assignmentViewModel;

		public MenuController (IntPtr handle) : base (handle)
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			assignmentViewModel.HoursChanged += (sender, e) => {
				if (IsViewLoaded) {
					timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
				}
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI we have to setup from code
			View.BackgroundColor = Theme.LinenLeft;
			tableView.Source = new TableSource ();

			var backButton = new UIBarButtonItem ("Assignments", UIBarButtonItemStyle.Bordered, (sender, e) => {
				var window = ServiceContainer.Resolve<UIWindow> ();
				window.RootViewController = ServiceContainer.Resolve<AssignmentsController> ();
			});
			backButton.SetBackgroundImage (Theme.BackButton, UIControlState.Normal, UIBarMetrics.Default);
			backButton.SetTitleTextAttributes (new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
			navigationBar.TopItem.LeftBarButtonItem = backButton;

			timerBackground.Image = Theme.TimerBackground;
			timerLabelBackground.Image = Theme.TimerField;

			status.StatusChanged += (sender, e) => {
				//TODO: do something
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if (assignmentViewModel.ActiveAssignment != null) {
				status.Assignment = assignmentViewModel.ActiveAssignment;
				timerView.Hidden = false;
				timerLabel.Text = assignmentViewModel.Hours.ToString (@"hh\:mm\:ss");
				record.SetBackgroundImage (assignmentViewModel.Recording ? Theme.RecordActive : Theme.Record, UIControlState.Normal);
			} else {
				timerView.Hidden = true;
			}
			tableView.SelectRow (NSIndexPath.FromRowSection (0, 0), false, UITableViewScrollPosition.Top);
		}

		private class TableSource : UITableViewSource
		{
			readonly UITableViewCell summaryCell, mapCell, itemsCell, laborCell, expensesCell, confirmationCell;

			public TableSource ()
			{
				summaryCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				summaryCell.TextLabel.Text = "Summary";
				summaryCell.TextLabel.TextColor = UIColor.White;
				summaryCell.BackgroundColor = UIColor.Clear;	
				summaryCell.BackgroundView = new UIImageView { Image = Theme.LeftListTop };
				summaryCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListTopActive };

				mapCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				mapCell.TextLabel.Text = "Map";
				mapCell.TextLabel.TextColor = UIColor.White;
				mapCell.BackgroundColor = UIColor.Clear;	
				mapCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
				mapCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };

				itemsCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				itemsCell.TextLabel.Text = "Items";
				itemsCell.TextLabel.TextColor = UIColor.White;
				itemsCell.BackgroundColor = UIColor.Clear;	
				itemsCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
				itemsCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };

				laborCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				laborCell.TextLabel.Text = "Labor Hours";
				laborCell.TextLabel.TextColor = UIColor.White;
				laborCell.BackgroundColor = UIColor.Clear;	
				laborCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
				laborCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };

				expensesCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				expensesCell.TextLabel.Text = "Expenses";
				expensesCell.TextLabel.TextColor = UIColor.White;
				expensesCell.BackgroundColor = UIColor.Clear;
				expensesCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
				expensesCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };

				confirmationCell = new UITableViewCell (UITableViewCellStyle.Default, null);
				confirmationCell.TextLabel.Text = "Confirmations";
				confirmationCell.TextLabel.TextColor = UIColor.White;
				confirmationCell.BackgroundColor = UIColor.Clear;	
				confirmationCell.BackgroundView = new UIImageView { Image = Theme.LeftListEnd };
				confirmationCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListEndActive };
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return 6;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				switch (indexPath.Row) {
				case 0:
					return summaryCell;
				case 1:
					return mapCell;
				case 2:
					return itemsCell;
				case 3:
					return laborCell;
				case 4:
					return expensesCell;
				case 5:
					return confirmationCell;
				default:
					throw new IndexOutOfRangeException ();
				}
			}
		}
	}
}
