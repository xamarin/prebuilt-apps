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

using FieldService.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FieldService.WinRT.ViewModels;

namespace FieldService.WinRT.Views
{
    /// <summary>
    /// Flyout panel for adding signature to the assignment
    /// </summary>
    public sealed partial class AddSignatureFlyoutPanel : UserControl {
        AssignmentViewModel assignmentViewModel;
        public AddSignatureFlyoutPanel ()
        {
            this.InitializeComponent ();

            DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();
        }
    }
}
