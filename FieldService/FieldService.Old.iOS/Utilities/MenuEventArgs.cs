//
//  Copyright 2013  Xamarin Inc.
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
	/// EventArgs used for when the selection changes in MenuController
	/// </summary>
	public class MenuEventArgs : EventArgs
	{
		public MenuEventArgs ()
		{
			Animated = true;
		}

		public UITableView TableView {
			get;
			set;
		}

		public NSIndexPath IndexPath {
			get;
			set;
		}

		public bool Animated {
			get;
			set;
		}
	}
}

