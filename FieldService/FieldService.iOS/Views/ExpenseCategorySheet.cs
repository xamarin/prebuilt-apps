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
using FieldService.Data;

namespace FieldService.iOS
{
	public class ExpenseCategorySheet : UIActionSheet
	{
		readonly ExpenseCategory[] categories;

		public ExpenseCategorySheet ()
		{
			//Pull out all ExpenseCategory values
			categories = (ExpenseCategory[])Enum.GetValues (typeof(ExpenseCategory));

			foreach (ExpenseCategory type in categories) {
				AddButton (type.ToString ());
			}

			Dismissed += (sender, e) => {
				if (e.ButtonIndex != -1)
					Category = categories[e.ButtonIndex];
			};
		}

		/// <summary>
		/// The category the user selected, or null if none
		/// </summary>
		public ExpenseCategory? Category
		{
			get; 
			private set;
		}
	}
}

