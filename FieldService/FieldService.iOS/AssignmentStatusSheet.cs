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

namespace FieldService.iOS
{
	public class AssignmentStatusSheet : UIActionSheet
	{
		public AssignmentStatusSheet ()
		{
			foreach (AssignmentStatus status in Enum.GetValues (typeof(AssignmentStatus))) {
				AddButton (status.ToString ());
			}

			Dismissed += (sender, e) => Index = e.ButtonIndex;
		}
		
		public int Index {
			get;
			private set;
		}

		public AssignmentStatus Status {
			get { 
				if (Index == -1)
					return (AssignmentStatus)(-1);
				return (AssignmentStatus)Enum.GetValues (typeof(AssignmentStatus)).GetValue (Index);
			}
		}
	}
}

