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
//    limitations under the License.using System;using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using FieldService.Android.Utilities;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android
{
	/// <summary>
	/// Base activity for all activities, handles global OnPause/OnResume
	/// </summary>
	public class BaseActivity : Activity
	{
		readonly LoginViewModel loginViewModel = ServiceContainer.Resolve<LoginViewModel> ();

		protected override void OnPause ()
		{
			base.OnPause ();

			loginViewModel.ResetInactiveTime ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			if (loginViewModel.IsInactive) {
				var intent = new Intent (this, typeof(LoginActivity));
				intent.SetFlags (ActivityFlags.ClearTop);
				StartActivity (intent);
			}

			//Just to be safe
			loginViewModel.ResetInactiveTime ();
		}
	}
}