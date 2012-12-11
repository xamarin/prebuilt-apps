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
using FieldService.WinRT.ViewModels;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FieldService.WinRT.Views {
    public sealed partial class AddExpenseFlyoutPanel : UserControl {
        ExpenseViewModel expenseViewModel;
        public AddExpenseFlyoutPanel ()
        {
            this.InitializeComponent ();

            DataContext =
                expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();
        }
    }
}
