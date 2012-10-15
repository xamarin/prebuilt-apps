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
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace FieldService.iOS
{
	/// <summary>
	/// This controller holds the main menu for assignments
	/// </summary>
	public partial class MenuController : UITableViewController
	{
		public MenuController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI we have to setup from code
			tableView.BackgroundColor = Theme.LinenPattern;

			summaryCell.TextLabel.Text = "Summary";
			summaryCell.BackgroundColor = UIColor.Clear;	
			summaryCell.BackgroundView = new UIImageView { Image = Theme.LeftListTop };
			summaryCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListTopActive };

			mapCell.TextLabel.Text = "Map";
			mapCell.BackgroundColor = UIColor.Clear;	
			mapCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
			mapCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };

			itemsCell.TextLabel.Text = "Items";
			itemsCell.BackgroundColor = UIColor.Clear;	
			itemsCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
			itemsCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };

			laborCell.TextLabel.Text = "Labor Hours";
			laborCell.BackgroundColor = UIColor.Clear;	
			laborCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
			laborCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };

			expensesCell.TextLabel.Text = "Expenses";
			expensesCell.BackgroundColor = UIColor.Clear;
			expensesCell.BackgroundView = new UIImageView { Image = Theme.LeftListMid };
			expensesCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListMidActive };

			confirmationsCell.TextLabel.Text = "Confirmations";
			confirmationsCell.BackgroundColor = UIColor.Clear;	
			confirmationsCell.BackgroundView = new UIImageView { Image = Theme.LeftListEnd };
			confirmationsCell.SelectedBackgroundView = new UIImageView { Image = Theme.LeftListEndActive };
		}
	}
}
