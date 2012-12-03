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
using MonoTouch.UIKit;

namespace FieldService.iOS
{
	/// <summary>
	/// A really simple UISearchDisplayDelegate that could be used throughout the app on multiple UITableViews
	/// </summary>
	public class SearchDisplay : UISearchDisplayDelegate
	{
		private readonly UITableView tableView;
		private readonly ISearchSource dataSource;
		
		public SearchDisplay (UITableView tableView, ISearchSource dataSource)
		{
			this.tableView = tableView;
			this.dataSource = dataSource;
		}

		/// <summary>
		/// Sets ISearchSource.SearchText and reloads the UITableView
		/// </summary>
		public override bool ShouldReloadForSearchString (UISearchDisplayController controller, string forSearchString)
		{
			dataSource.SearchText = forSearchString.ToLower ();
			tableView.ReloadData ();
			
			return true;
		}
	}
}

