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
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.iOS
{
	/// <summary>
	/// A UIActionSheet that provides AssignmentStatus as a selection
	/// </summary>
	public class AssignmentStatusSheet : UIActionSheet
	{
		readonly AssignmentViewModel assignmentViewModel;

		public AssignmentStatusSheet ()
		{
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();

			//Loop through the list of statuses you can change an assignment to
			foreach (AssignmentStatus status in assignmentViewModel.AvailableStatuses) {
				AddButton (status.ToString ());
			}

			Dismissed += (sender, e) => {
				if (e.ButtonIndex != -1)
					Status = assignmentViewModel.AvailableStatuses[e.ButtonIndex];
			};
		}
		
		/// <summary>
		/// The selected status
		/// </summary>
		public AssignmentStatus? Status {
			get;
			private set;
		}
	}
}

